// This component observes and reacts to changes in the modal store.
import { observer } from "mobx-react-lite";
// Custom hook to access the application's store.
import { useStore } from "../../stores/store";
// Modal component from Semantic UI React library.
import { Modal } from "semantic-ui-react";

// ModalContainer is an observer component that displays a modal dialog.
export default observer(function ModalContainer() {
  // Destructure modalStore from the application's store.
  const { modalStore } = useStore();

  return (
    // Render a modal dialog with properties controlled by modalStore.
    <Modal
      open={modalStore.modal.open}
      onClose={modalStore.closeModal}
      size="mini"
    >
      <Modal.Content>
        {/* Render the content of the modal body from modalStore. */}
        {modalStore.modal.body}
      </Modal.Content>
    </Modal>
  );
});
