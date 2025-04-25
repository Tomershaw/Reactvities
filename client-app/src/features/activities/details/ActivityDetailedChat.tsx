// This component displays a detailed chat section for a specific activity.
// Props:
// - activityId: The ID of the activity for which the chat is displayed.

// Wraps the component with MobX observer to react to observable state changes.
import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Segment, Header, Comment, Loader } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { Link } from "react-router-dom";
import { Formik, Form, Field, FieldProps } from "formik";
import * as Yup from "yup";
import { formatDistanceToNow } from "date-fns";

interface Props {
  activityId: string; // The ID of the activity to fetch and display comments for.
}

export default observer(function ActivityDetailedChat({ activityId }: Props) {
  const { commentStore } = useStore(); // Access the comment store from MobX.

  useEffect(() => {
    if (activityId) {
      commentStore.createHubConnection(activityId); // Establish a SignalR connection for real-time comments.
    }
    return () => {
      commentStore.clearComments(); // Clean up comments when the component unmounts.
    };
  }, [commentStore, activityId]);

  return (
    <>
      {/* Header section for the chat */}
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>Chat about this event</Header>
      </Segment>

      {/* Chat input and comments section */}
      <Segment attached clearing>
        <Formik
          onSubmit={
            (values, { resetForm }) =>
              commentStore.addComment(values).then(() => resetForm()) // Add a comment and reset the form.
          }
          initialValues={{ body: "" }}
          validationSchema={Yup.object({
            body: Yup.string().required(" is required"), // Validate that the comment body is not empty.
          })}
        >
          {({ isSubmitting, isValid, handleSubmit }) => (
            <Form className="ui form">
              <Field name="body">
                {(props: FieldProps) => (
                  <div style={{ position: "relative" }}>
                    <Loader active={isSubmitting} />{" "}
                    {/* Show loader while submitting. */}
                    <textarea
                      placeholder="Enter your comment (Enter to submit, SHIFT + enter for new line)"
                      rows={2}
                      {...props.field}
                      onKeyDown={e => {
                        if (e.key === "Enter" && e.shiftKey) {
                          return; // Allow new line with SHIFT + Enter.
                        }
                        if (e.key === "Enter" && !e.shiftKey) {
                          e.preventDefault();
                          isValid && handleSubmit(); // Submit the form on Enter.
                        }
                      }}
                    />
                  </div>
                )}
              </Field>
            </Form>
          )}
        </Formik>

        {/* Display the list of comments */}
        <Comment.Group>
          {commentStore.comments.map(comment => (
            <Comment key={comment.id}>
              <Comment.Avatar src={comment.image || "/assets/user.png"} />{" "}
              {/* User avatar */}
              <Comment.Content>
                <Comment.Author as={Link} to={`/profiles/${comment.username}`}>
                  {comment.displayName}
                </Comment.Author>{" "}
                {/* Link to user profile */}
                <Comment.Metadata>
                  <div>{formatDistanceToNow(comment.createAt)}</div>{" "}
                  {/* Time since comment creation */}
                </Comment.Metadata>
                <Comment.Text style={{ whiteSpace: "pre-wrap" }}>
                  {comment.body}
                </Comment.Text>{" "}
                {/* Comment text */}
              </Comment.Content>
            </Comment>
          ))}
        </Comment.Group>
      </Segment>
    </>
  );
});
