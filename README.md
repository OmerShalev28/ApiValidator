# API Document Validator

## Overview

This project is a **C# console application** that validates an external API by generating random documents, submitting them, verifying their creation, and logging relevant insights. The application interacts with the API to create, track, and validate document entries while maintaining detailed logs.

## Features

✅ **Generate Random Document** – Creates a valid document following API specifications.\
✅ **API Integration** – Communicates with the API to create and validate document entries.\
✅ **Logging** – Uses **log4net** to log key operations, requests, responses, and errors.\
✅ **Error Handling** – Handles API failures, invalid responses, and exceptions gracefully.\
✅ **Configuration Management** – Uses `settings.json` for API settings and authentication.\
✅ **Status Polling** – Repeatedly checks for document creation status until confirmation.

---

## Setup Instructions

### **1. Clone the Repository**

```sh
git clone <repository-url>
cd <project-directory>
```

### **2. Restore Dependencies**

```sh
dotnet restore
```

### **3. Configure Settings**

Edit the `settings.json` file to update API details:

```json
{
  "BaseUrl": "<API_BASE_URL>",
  "ProviderUserToken": "<PROVIDER_USER_TOKEN>",
  "BearerToken": "<BEARER_TOKEN>",
  "ContentType": "application/json",
  "CreateEndPoint": "documents/create",
  "GetEndPoint": "documents/",
  "FetchEndPoint": "documents/search"
}
```

### **4. Build the Project**

```sh
dotnet build
```

### **5. Run the Application**

```sh
dotnet run
```

The application will:

- Generate a random document
- Send it to the API for creation
- Poll for the document creation status
- Validate that the document exists in the retrieved list

---

## Project Structure

📂 **Program.cs** – Main application logic: API requests, responses, and status polling.\
📂 **DocumentGenerator.cs** – Generates a random document with configurable and randomized data.\
📂 **Customer.cs, Document.cs, DocumentItems.cs** – Model classes representing document structure.\
📂 **settings.json** – Configuration file storing API settings and authentication tokens.\
📂 **log4net.config** – Configures logging to console and file.\
📂 **application.log** – Sample log file from a successful run.

---

## API Interaction

This application interacts with the API using the following endpoints:

- **Create a Document** → `POST [BaseUrl]/[CreateEndPoint]`
- **Poll for Document Status** → `GET [BaseUrl]/[GetEndPoint]{requestId}`
- **Fetch Document List** → `POST [BaseUrl]/[FetchEndPoint]`

---

## Error Handling

🔹 **HTTP Errors** – Checks HTTP response codes and throws exceptions for failures.\
🔹 **Validation Errors** – Logs responses containing validation issues.\
🔹 **Retries** – Implements retry logic for polling document status using `Task.Delay`.

---

## Assumptions & Limitations

⚠️ Assumes the API follows the provided documentation.\
⚠️ Basic error handling implemented but could be extended.\
⚠️ Limited to retrieving and validating created documents.\
⚠️ Currently does not support all document types.
