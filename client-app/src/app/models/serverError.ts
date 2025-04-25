// Represents an error response from the server
export interface ServerError {
  statusCode: number; // HTTP status code of the error
  message: string; // Error message provided by the server
  detalis: string; // Additional details about the error
}
