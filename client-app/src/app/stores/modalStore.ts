import { makeAutoObservable } from "mobx";

interface Modal {
  open: boolean;
  body: JSX.Element | null;
}

// MobX store for managing modal state
export default class ModalStore {
  // Represents the modal's open state and content
  modal: Modal = {
    open: false,
    body: null,
  };

  constructor() {
    makeAutoObservable(this); // Enables reactive state management
  }

  // Opens the modal with the specified content
  openModal = (content: JSX.Element) => {
    this.modal.open = true;
    this.modal.body = content;
  };

  // Closes the modal and clears its content
  closeModal = () => {
    this.modal.open = false;
    this.modal.body = null;
  };
}
