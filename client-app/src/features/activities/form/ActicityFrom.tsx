// This component renders a form for creating or editing an activity.
// It uses Formik for form handling and validation, and MobX for state management.

// Import necessary hooks, components, and utilities.
import { useEffect, useState } from "react";
import { Segment, Button, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores/store";
import { observer } from "mobx-react-lite";
import { Link, useNavigate, useParams } from "react-router-dom";
import { ActivityFormValues } from "../../../app/models/activity";
import LoadingComponent from "../../../app/layout/loadingComponent";
import { v4 as uuid } from "uuid";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import MyTextInput from "../../../app/common/form/MyTextInput";
import MyTextArea from "../../../app/common/form/MyTextArea";
import { categoryOptions } from "../../../app/common/options/CategoryOptions";
import MySelectInput from "../../../app/common/form/MySelectInput";
import MyDateInput from "../../../app/common/form/MyDateInput";

export default observer(function ActivityFrom() {
  // Access the activity store and its methods from MobX.
  const { activityStore } = useStore();
  const { createActivity, updateActivity, loadActivity, loadingInitial } =
    activityStore;

  // Extract the activity ID from the URL parameters and initialize navigation.
  const { id } = useParams();
  const navigate = useNavigate();

  // Define validation schema for the form using Yup.
  const validationSchema = Yup.object({
    title: Yup.string().required("The activity title is required"),
    description: Yup.string().required("The activity description is required"),
    category: Yup.string().required(),
    date: Yup.string().required(),
    venue: Yup.string().required(),
    city: Yup.string().required(),
  });

  // Initialize the activity state with default values.
  const [activity, setActivity] = useState<ActivityFormValues>(
    new ActivityFormValues()
  );

  // Load activity details if an ID is provided, and set the form state.
  useEffect(() => {
    if (id)
      loadActivity(id).then(activity =>
        setActivity(new ActivityFormValues(activity))
      );
  }, [id, loadActivity]);

  // Handle form submission for creating or updating an activity.
  function HandleFromSubmit(activity: ActivityFormValues) {
    if (!activity.id) {
      const newActivity = {
        ...activity,
        id: uuid(),
      };
      createActivity(newActivity).then(() =>
        navigate(`/activities/${newActivity.id}`)
      );
    } else {
      updateActivity(activity).then(() =>
        navigate(`/activities/${activity.id}`)
      );
    }
  }

  // Show a loading component while the activity is being loaded.
  if (loadingInitial) return <LoadingComponent content="Loading activity" />;

  return (
    <Segment clearing>
      {/* Render the form header */}
      <Header content="Activity Details" sub color="teal" />

      {/* Formik form for activity details */}
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={activity}
        onSubmit={values => HandleFromSubmit(values)}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            {/* Input fields for activity details */}
            <MyTextInput name="title" placeholder="Title" />
            <MyTextArea rows={3} placeholder="Description" name="description" />
            <MySelectInput
              options={categoryOptions}
              placeholder="Category"
              name="category"
            />
            <MyDateInput
              placeholderText="Date"
              name="date"
              showTimeSelect
              timeCaption="time"
              dateFormat="MMMM d, yyyy h:mm aa"
            />

            {/* Input fields for location details */}
            <Header content="Location Details" sub color="teal" />
            <MyTextInput placeholder="City" name="city" />
            <MyTextInput placeholder="Venue" name="venue" />

            {/* Submit and cancel buttons */}
            <Button
              disabled={isSubmitting || !dirty || !isValid}
              loading={isSubmitting}
              floated="right"
              positive
              type="submit"
              content="Submit"
            />
            <Button
              as={Link}
              to="/activities"
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
