# ReviewInsights

An AI-powered customer review analyzer built with ASP.NET Core 8, React, TypeScript, SQL Server, and Google Gemini.

ReviewInsights analyzes customer feedback and identifies its sentiment, category, and a concise summary. It stores analyzed reviews in a database and displays them in a dashboard.

## Features

- **AI-powered analysis:** Analyze customer reviews using Google Gemini.
- **Sentiment classification:** Classify reviews as Positive, Neutral, or Negative.
- **Category classification:** Identify issues such as Bugs, Pricing, Usability, Performance, Customer Support, and Feature Requests.
- **Concise summaries:** Generate short summaries of customer feedback.
- **Persistent storage:** Store reviews and their analysis results in SQL Server.
- **Review dashboard:** Browse reviews, view summary statistics, and filter by sentiment.
- **REST API:** Retrieve existing reviews and submit new reviews for analysis.

## Tech stack

| Component | Technology |
|---|---|
| Backend | ASP.NET Core 8 Web API |
| Language | C# |
| Frontend | React, TypeScript, Vite |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core |
| AI provider | Google Gemini API |
| Dependency injection | Built-in ASP.NET Core DI |

## Prerequisites

Install the following before running the project:

1. **Git** — to clone the repository.
2. **.NET 8 SDK** — to build and run the backend.
3. **Node.js and npm** — to install and run the frontend. Use a currently supported Node.js LTS release compatible with the Vite project.
4. **Microsoft SQL Server** — SQL Server Express or another compatible SQL Server instance.
5. **SQL Server Management Studio (SSMS)** — optional, but useful for inspecting the database.
6. **Google Gemini API key** — required for AI-powered review analysis. Obtain one from [Google AI Studio](https://aistudio.google.com/).

Verify your installations:

```bash
git --version
dotnet --version
node --version
npm --version
```

The .NET SDK should report version 8.x.

## 1. Clone the repository

Open a terminal and run:

```bash
git clone https://github.com/Rahulck43/ReviewInsights.git
cd ReviewInsights
```

The repository contains the backend and frontend projects.

```text
ReviewInsights/
├── ReviewInsights.Api/
│   ├── Controllers/
│   ├── Data/
│   ├── Services/
│   ├── Program.cs
│   └── ...
├── review-insights-ui/
│   ├── src/
│   ├── package.json
│   ├── vite.config.ts
│   └── ...
├── .gitignore
└── README.md
```

## 2. Configure SQL Server

Make sure your SQL Server instance is installed and running.

The application uses a connection string named `DefaultConnection`.

For local development, configure it through .NET User Secrets rather than committing machine-specific connection details to the repository.

Open a terminal:

```bash
cd ReviewInsights.Api
dotnet user-secrets init
```

Configure the connection string for your own SQL Server instance.

For a typical local SQL Server Express installation using Windows authentication:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost\SQLEXPRESS;Database=ReviewInsightsDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

Adjust the server name to match your SQL Server installation.

For example:
- `localhost\SQLEXPRESS` for a local SQL Server Express instance.
- `localhost` for a local default SQL Server instance.
- Your configured server name for a different SQL Server installation.

### Create or update the database

Install the EF Core command-line tool:

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

If the tool is already installed, update it to a compatible version if needed.

From the backend project directory, apply the migrations:

```bash
dotnet ef database update
```

This creates or updates the database schema defined by the project's migrations.

## 3. Configure the Gemini API key

ReviewInsights uses Google Gemini to analyze customer feedback.

From the `ReviewInsights.Api` directory, configure your own key:

```bash
dotnet user-secrets set "Gemini:ApiKey" "YOUR_GEMINI_API_KEY"
```

Replace `YOUR_GEMINI_API_KEY` with your actual key.

The backend reads the key through ASP.NET Core configuration, using the `Gemini:ApiKey` setting.

To verify that the setting exists:

```bash
dotnet user-secrets list
```

### Why use User Secrets?

User Secrets keeps local development credentials outside the Git repository. Each developer can configure their own API key and database connection string.

## 4. Run the backend API

From the backend directory:

```bash
dotnet restore
dotnet build
dotnet run
```

### Verify the API

Open the following URL in your browser:

```text
http://localhost:5057/api/reviews
```

The endpoint should return a JSON array containing the saved reviews. An empty array is also valid if no reviews have been created yet.

Keep the backend terminal running.

## 5. Configure the frontend

Open a second terminal and navigate to the frontend directory:

```bash
cd review-insights-ui
```

Create a file named `.env.local` in that directory:

```dotenv
VITE_API_BASE_URL=http://localhost:5057
```

If the backend is listening on a different port, update the URL accordingly.

## 6. Install and run the frontend

From the `review-insights-ui` directory, run:

```bash
npm install
npm run dev
```

Vite will display a local development URL, usually:

```text
http://localhost:5173
```

Open that URL in your browser.

Keep the frontend and backend running in separate terminals.

## 7. Test the application

Once both applications are running:

1. Open the frontend dashboard.
2. Confirm that existing reviews load from the API.
3. Enter a new customer review in the text area.
4. Click **Add review**.
5. Wait for the backend to process the request and return the analysis.
6. Verify that the sentiment, category, and summary appear in the dashboard.
7. Refresh the page and verify that the saved review remains available.
8. Use the sentiment filter to browse Positive, Neutral, and Negative reviews.

Example test review:

> The application is easy to use, but reports load slowly, image uploads frequently fail, and the subscription is becoming too expensive. Customer support was polite but did not resolve my problem.

The AI-generated result may vary, but it should reflect the main issues expressed in the review.

## API endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/reviews` | Retrieve saved reviews |
| POST | `/api/reviews` | Submit a review for processing and persistence |

### Example request

`POST /api/reviews`

Content-Type: `application/json`

```json
{
  "reviewText": "The app is useful, but it crashes when I upload a photo."
}
```

The endpoint returns the review data according to the API's response contract.

## Configuration reference

| Setting | Purpose | Recommended location |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | SQL Server connection | User Secrets locally |
| `Gemini:ApiKey` | Gemini authentication | User Secrets locally |
| `VITE_API_BASE_URL` | Backend API address | Frontend `.env.local` |

The backend uses ASP.NET Core configuration to read its settings. User Secrets are available in the Development environment.

## Troubleshooting

### SQL Server connection errors

- Confirm that SQL Server is running.
- Check the instance name in `ConnectionStrings:DefaultConnection`.
- Verify Windows authentication or the configured SQL login.
- Confirm that the database exists and the schema has been applied.
- Check the connection string and certificate settings.

### Gemini API errors

- Verify that `Gemini:ApiKey` is configured in User Secrets.
- Ensure the key is valid and the selected Gemini model is available.
- Check the backend terminal for provider errors or rate-limit responses.
- Never put the API key in frontend code or a Vite environment variable.

### CORS errors

The backend must allow requests from the frontend's exact origin.

The default development origin is:

```text
http://localhost:5173
```

If Vite starts on another port, update the backend's CORS policy to match the actual frontend origin. Restart the backend after changing the policy.

### Reviews do not load

- Confirm that the backend is running.
- Check `VITE_API_BASE_URL` in `.env.local`.
- Confirm that `/api/reviews` works directly in the browser.
- Restart Vite after modifying `.env.local`.
- Inspect the browser's Developer Tools console and Network tab.

### Database tables are missing

Apply the project's Entity Framework Core migrations if they are included. If migrations are absent, the schema initialization process needs to be established before a new developer can run the application against an empty database.

## Development workflow

Run the backend and frontend in separate terminals:

**Terminal 1 — Backend**

```bash
cd ReviewInsights.Api
dotnet run
```

**Terminal 2 — Frontend**

```bash
cd review-insights-ui
npm run dev
```

Install dependencies with `dotnet restore` and `npm install` when setting up a fresh clone or when project dependencies change.

---

Built with ASP.NET Core, React, SQL Server, and Google Gemini.
