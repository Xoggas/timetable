## 🕒 School Timetable Management System  

Welcome to the **Timetable Management System**! This project is designed to help schools efficiently manage their schedules and bell timings. It consists of two main modules: one for managing class schedules and another for managing bell timings.  

## 🔧 Features  

### Schedule Management Module  
- 🔎 **Quick Access to Lessons**: View a list of all lessons for fast navigation.  
- 📅 **Weekly Schedule Table**: Organized table showing lessons for each day of the week.  
- 🔒 **Backup and Restore**: Create backups of the schedule table and restore it when needed.  

### Bell Management Module  
*(Details coming soon!)*  

## 🛠️ Technology Stack  
- 🖥️ **ASP.NET Core Web API**: A powerful framework for building web APIs.  
- ⚡ **SignalR**: For real-time communication features.  
- 🗄️ **MongoDB**: A NoSQL database for efficient data storage and retrieval.  
- 🌐 **Blazor**: A powerful frontend framework for building web UI applications.  

## 🐋 DockerHub Repositories  
- 🖥️ **Frontend**: [Frontend Repository](https://hub.docker.com/repository/docker/xoggas/timetable.frontend/general)  
- 🔧 **API**: [API Repository](https://hub.docker.com/repository/docker/xoggas/timetable.api/general)  

## 🚀 Deployment Instructions  

To deploy the **Timetable Management System**, follow these steps.  

### Prerequisites:  
- Ensure you have **Docker** and **docker-compose** installed on your server.  

### Steps to Deploy:  

1. Create a `docker-compose.yml` file in the desired directory with the following content:  

   ```yaml
   version: '3.8'

   services:
     mongo:
       image: mongo
       ports:
         - "27017:27017"
       volumes:
         - mongo-data:/data/db
       networks:
         - custom-network

     timetable.api:
       image: xoggas/timetable.api
       ports:
         - "5000:8080"
       depends_on:
         - mongo
       networks:
         - custom-network
       environment:
         - ASPNETCORE_ENVIRONMENT=Production

     timetable.frontend:
       image: xoggas/timetable.frontend
       ports:
         - "80:8080"
       depends_on:
         - timetable.api
       networks:
         - custom-network
       environment:
         - ASPNETCORE_ENVIRONMENT=Production

   networks:
     custom-network:
       external: false

   volumes:
     mongo-data:
   ```

2. Deploy the application using `docker-compose`:  

   ```bash
   docker-compose up -d
   ```

3. Access the application:  
   - **Frontend**: Open your browser and go to [http://your-server-ip](http://your-server-ip).  
   - **API**: The API is accessible at [http://your-server-ip:5000](http://your-server-ip:5000).  

### Stopping the Application:  
To stop the services, run:  
```bash
docker-compose down
```  

## License  

This project is licensed under the MIT License. See the `LICENSE` file for more details.  

---  

Happy coding! ✨
