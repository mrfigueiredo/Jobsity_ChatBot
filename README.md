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

1. The project could not be completed due to problems beyond my control. Due to storms in my region, there were problems with the power distribution network starting on Friday and throughout the weekend.
2. Due to the previous item, the focus was on creating the general functionality, specified in the mandatory, with some Bonus points incorporated into the project. However, not all the functionalities were properly tested, so issues might be found running.
3. Among the items completed are:

### Mandatory

- Allow registered users to log in and talk with other users in a chatroom.
- Allow users to post messages as commands into the chatroom with the following format /stock=stock_code .
- Create a decoupled bot that will call an API using the stock_code as a parameter.
- The bot should parse the received CSV file and then it should send a message back into the chatroom using a message broker like RabbitMQ. The message will be a stock quote using the following format: “APPL.US quote is $93.42 per share”. The post owner will be the bot.
- Have the chat messages ordered by their timestamps and show only the last 50 messages.

### Bonus

- Have more than one chatroom.
- Use .NET identity for users authentication

---
