# Bakaláři client
Client for Bakaláři app (alpha)
- features:
  - currently only dynamic schedule displaying
  
- in future:
  - adding tooltip for subjects such as location, teacher, substitution (suplování), etc.
  - displaying special subject events (with green mark)
  - displaying absence
  - displaying komens


# How does it work?
Directly send post request to specified login url and store session and login cookies locally. Then send request to get schedule and display that to user.



# Setup
1. Dowload release from right panel
2. Create directories as shown
    ```
    parent_directory
        - config.json (step 3)
        - some_directory
            - Debug_directory
                - exe files, etc.
    
    NOTE: config.json must be exactly 2 directory higher than `BakalariClient.exe` file
    ```
3. Create `config.json` file as shown above and paste this in it replacing domain with URL of login page and credentials with your credentials
   ```
    {
        "domain": "https://your_school_bakalari_web_domain.domain",
        "credentials": {
            "username": "your_username",
            "password": "your_password"
        } 
    }
   ```
4. In `Debug/BakalariClient.exe` right click at `Debug/BakalariClient.exe` and create shortcut


NOTE: THIS APPLICATION IS RUNNING LOCALLY AND IS NOT SENDING YOUR CREDENTIALS TO THIRD PARTY SERVICES, JUST BAKALÁŘI WEBSITE
