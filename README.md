# 📈 ABX Client Application

This is a **C# console application** that connects to a mock stock exchange server (**ABX Mock Exchange Server**) via **TCP**, requests and receives stock ticker data using a custom binary protocol, handles missing packets, and generates a clean, ordered JSON output.

---

## 🧠 About the ABX Mock Exchange Server

The **ABX Mock Exchange Server** is a lightweight **Node.js TCP server** included in this repo as `abx_exchange_server.zip`. It simulates a real-time stock exchange by sending binary packets containing stock ticker data.

---

## 🚀 How to Run Locally

### ✅ 1. Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download)
- [Node.js (v16+)](https://nodejs.org/)
- Any IDE (Visual Studio, Rider, VS Code)

---

### 📂 2. Clone This Repository

```bash
git clone https://github.com/PriyankDonda/ABX_Client_App.git
cd ABX_Client_App
```

---

### 🧪 3. Run ABX Mock Exchange Server

#### 1. Extract and Set Up

Unzip `abx_exchange_server.zip`:

```bash
unzip abx_exchange_server.zip -d abx_exchange_server
cd abx_exchange_server
```

#### 2. Install dependencies

```bash
npm install
```

#### 3. Start the server

```bash
node main.js
```

---

### 💻 4. Run ABX Client Application

In a **new terminal**, navigate to the main directory:

```bash
cd AbxClientApp
dotnet run --project AbxClientApp.csproj
```

If everything works correctly, you will see:

```
[INFO] ABX Client completed successfully.
[INFO] Output written to: /path/to/AbxClientApp/bin/output/output.json
```

---

## 📊 Output

You’ll find the final JSON data at:

```
output/output.json
```

This file contains all packets received, ordered by sequence number.

