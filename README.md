# PlatformNext API C# Console App

This repository provides a simple, self-contained **C# console application** designed to help developers understand how to connect to and call common **Profituity REST API** endpoints using mock data.

The app covers authentication, merchant portal payment operations, and reporting using structured fetch calls and real-time responses in the console.

---

## What This App Demonstrates

This app connects to PlatformNext's API and walks through the following operations:

- Creating a payment
- Creating bulk payments
- Creating a payment template
- Creating a payment from a template
- Fetching all payments
- Fetching a payment status-date report
- Canceling a payment
- Refunding a payment

All requests are authenticated via **OAuth2 password grant**, and use user secrets for secure configuration.

---

## Prerequisites

Before you begin, make sure you have:

- [.NET 6+ SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)
- A Profituity **sandbox API account**
  - Username
  - Password
  - Merchant ID

---

## Project Structure

- `Program.cs` – Main entry point that coordinates API calls
- `AuthService.cs` – Handles authentication and token management
- `Models/` – DTO classes for each request payload
- `Services/` – API call logic grouped by responsibility
  - `PaymentService.cs`
  - `PaymentTemplateService.cs`
- `Helpers/` – Random data generators for DTOs
  - `PaymentDtoGenerator.cs`
  - `PaymentTemplateDtoGenerator.cs`
  - `DateHelper.cs`
- `appsettings.json` – Optional file structure for local values (user secrets recommended)

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/profituity/PlatformNext_MerchantPortal_SampleC.git
cd PlatformNext_MerchantPortal_SampleC
```

### 2. Install Dependencies
Run from the project root:
```bash
dotnet restore
```

### 3. Set Up User Secrets
We recommend storing your API credentials (username, password, and merchant id) securely using User Secrets.

```bash
{
    "ApiSettings": {
      "ApiUrl": "https://sandbox.dev.profituity.com",
      "Username": "your_username",
      "Password": "your_password",
      "MerchantId": "your_merchant_id"
    }
}
```

### 4. Run the Application
```bash
dotnet run
```
The app will authenticate, call each endpoint, and print results to the console.


### 5. Sample Output

```bash
Single Payment Created: { id: "...", status: "Scheduled" }
Bulk Payments Created: [{ id: "..." }, ...]
Payment Template Created: { id: "...", name: "Monthly Plan" }
Payment Created from Template: { id: "...", amount: 100 }
Payments Retrieved: [ ... ]
Status Date Payment Report (start to end): [ ... ]
Cancel Flow: { status: 204, message: 'Payment canceled successfully' }
Refund Flow: { id: "...", amount: 1 }
```

---

## Customizing
To test different endpoints, edit RunAllAsync() in Program.cs or temporarily comment/uncomment specific calls.

You can also change data generators inside the Helpers/ folder to fit your testing needs and modify all data to see your results as needed.

## Need Help?

Contact the Profituity team or refer to the [REST API documentation](https://help.profituity.com/ck/introducing-our-rest-api) for more details.
