# WeCasa
CECS 491A/B Project

# Kanban Board
https://trello.com/invite/b/flAWDMWj/6c8c5e7eb8e06ec0e0f2c69fedd6f343/cecs-491a

## Project Setup
```
|___WeCasa
    |__src
      |__Frontend/HAGSJP.WeCasa.Frontend
         |__HAGSJP.WeCasa.Frontend.csproj
         |__ClientApp
            |__public
            |__src
            |__README.md
            |__package.json
```
To run locally, open the HAGSJP.WeCasa.sln file in Visual Studio and run the following in the terminal:
1. ```git clone https://github.com/githelsui/WeCasa.git```
2. Create a copy of config.template.json file in HAGSJP.WeCasa.sqlDataAccess 
3. Rename file to config.json and add database configuration
4. ```cd Frontend/HAGSJP.WeCasa.Frontend/ClientApp```
5. ```npm install``` 

Run the HAGSJP.WeCasa.Frontend.csproj file