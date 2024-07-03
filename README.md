# ChatApp

ChatApp is a real-time chat application with integrated stock quote functionality using RabbitMQ for messaging.

## Setup

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- [RabbitMQ](https://www.rabbitmq.com/download.html) server running locally

### Installation

1. Clone the repository:

   ```sh
   git clone https://github.com/your/repository.git
   cd ChatApp
   ```

2. Restore dependencies and build the solution:

   ```sh
   dotnet restore
   dotnet build
   ```

3. Update the database connection string in `appsettings.json` under `"ConnectionStrings"`.

4. Start the application:

   ```sh
   dotnet run --project ChatApp
   ```

5. Navigate to `https://localhost:5169/Account/Login` in your web browser to use the application.

## Functionality

- **Real-time Chat**: Users can join chat rooms and send messages in real-time using SignalR.
- **Stock Quotes**: Enter `/stock=<STOCK_CODE>` in the chat to request a stock quote (e.g., `/stock=APPL`).

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- SignalR
- RabbitMQ
- Razorpages

---

# StockQuoteBot

StockQuoteBot is a background service that listens for stock quote requests from the ChatApp via RabbitMQ.

## Setup

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) installed
- [RabbitMQ](https://www.rabbitmq.com/download.html) server running locally

### Installation

1. Navigate to the StockQuoteBot project directory:

   ```sh
   cd StockQuoteBot
   ```

2. Restore dependencies and build the project:

   ```sh
   dotnet restore
   dotnet build
   ```

3. Start the bot:
   ```sh
   dotnet run
   ```

## Functionality

- **Stock Quote Listener**: Listens for stock quote requests from RabbitMQ, fetches the stock quote from a web service, and prints the quote to the console.

## Technologies Used

- .NET Core Console Application
- RabbitMQ
- HttpClient for API requests

---

### Final Considerations

- The UI has some issues (AKA: i didn't know how to handle the previous messages, the API is prepared for that, my front-end skills couldn't match).
- For time issues, i was not able to handle exceptions and messages more reliable with a middleware for centralizing it.
- No unit tests were created because of the same time issue.
- Profiler run didn't show any leak or resource overload, however this test was made with jmeter, so was not completely accurate.
- More than one bot can be added to the same message queue for horizontal scaling.

### Possible Next steps:

- Improve architecture, probably event-driven, for easy scalability.
- Add unit and integration tests.
- Fix message and http request from the bot service.
- Study the possibility to create a queue for responses with a consumer inside the ChatApp side.

### Completed goals:

#### Mandatory

- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format /stock=stock_code .
- Create a decoupled bot that will call an API using the stock_code as a parameter.
- The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: “APPL.US quote is $93.42 per share”. The post owner will be the bot.

#### Bonus

- Have more than one chatroom.
- Use .NET identity for users authentication

---
