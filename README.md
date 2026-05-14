# FinGuardAI: Turning Static Financial Policies into Actionable Intelligence

FinGuardAI is a state-of-the-art AI governance system designed to transform static PDF financial policies into an interactive, intelligent advisory agent. Developed by team Binary for **#SalamHack_2026**, this project ensures financial integrity through real-time policy enforcement.

---

## Description

Traditional financial policies often exist as "dead" PDF documents that are difficult for employees to follow and time-consuming for managers to audit. FinGuardAI bridges this gap by utilizing **Retrieval-Augmented Generation (RAG)** to provide an "always-on" advisory agent.

The system allows employees to submit financial requests while the AI Agent checks them against existing company policies in real-time, providing managers with structured recommendations and text citations from the law. This reduces human error, prevents policy violations, and ensures complete transparency.

## Installation

To run FinGuardAI locally, follow these steps:

Clone the Repository

```bash
git clone https://github.com/AHMED-BINATTASH/FinGuardAI.git
cd FinGuardAI
```

## Configure Environment Variables

### Create an appsettings.json file in the API project and add your keys:


```bash
{
  "OpenAiApiKey": "YOUR_OPENAI_API_KEY",
  "PineconeApiKey": "YOUR_PINECONE_API_KEY",
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=FinGuardDB;..."
  }
}
```

## Restore & Run

```bash
dotnet restore
dotnet run --project FinGuardAI.API
```

## Usage

The system works as follows:

1. **Employee Submission:** A user submits a purchase or expense request.  
2. **AI Audit:** The agent retrieves the relevant policy from the Vector Database and compares it with the request.  
3. **Advisory Recommendation:** The agent generates a "Yellow Flag" or "Green Flag" recommendation card for the CFO, citing the specific policy article.  
4. **CFO Decision:** The manager makes the final decision based on the AI's evidence-backed analysis.

---

## System Flow

- Employee submits request → AI checks policy → Recommendation generated → CFO approves/denies

---

## Features

- **RAG-Powered Intelligence:** Uses Semantic Search to understand the intent of financial requests, not just keywords.  
- **Zero-Hallucination Guardrails:** Strict system prompts ensure the AI only speaks based on uploaded corporate documents.  
- **Structured Output:** Provides clean JSON data for seamless frontend integration.  
- **Explainability:** Every recommendation includes a direct quote from the company policy.  
- **Financial Integrity:** All actions are logged in an immutable audit trail.

---

## API Reference

**POST** `/api/Agent/runRequest`

**Request Body:**

```json
{
  "sessionId": "user-guid-123",
  "message": "I need to purchase a new laptop for $1,500 under the tech equipment budget."
}
```

## Technologies Used

- **Language:** C# / .NET 8  
- **Orchestration:** Microsoft Semantic Kernel  
- **LLM Brain:** GPT-4o-mini  
- **Databases:** SQL Server (Structured) & Pinecone (Vector)  
- **Architecture:** Clean Architecture / Three-Tier

---

## Contributing

We welcome contributions to team Binary's project:

1. Fork the Project  
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)  
3. Commit your Changes (`git commit -m 'Add AmazingFeature'`)  
4. Push to the Branch (`git push origin feature/AmazingFeature`)  
5. Open a Pull Request

---

## Tests

To run the unit and integration tests:

```bash
dotnet test
```

## License

Distributed under the **MIT License**.

---

## Acknowledgements

- Dr. Mohammed Abu-Hadhoud: For mentorship and the software engineering roadmap  
- SalamHack 2026: For the platform to innovate  
- Team Binary: For the collaborative spirit and hard work
