# Web API, RESTful, JWT 🔑⏰
Save users with/without roles + generate access tokens (bearer) with 60 minute expiration time + ADMINISTRATOR profile can manage users and their roles + user and roles information is set manually (for practice purposes because ASP does it for us).

The implementation of this API and JWT Bearer is for **practice/study purposes in web api and basic knowledge of JWT**. Basic authentication for administrators to manipulate the user database. Do not use for productive purposes; It is recommended that for production level authentication be done through a cloud provider such as Auth0, AD or third-party libraries such as Duende IdentityServer. The management of user information should be delegated to the Identity library which is efficient in handling the persistence of user information.


**It is stated that in-house authentication does not add value and are functions that other providers can do more securely and efficiently.**

```bash
Clean Architecture
```

# 🛠 Skills
* Api con .Net
* RESTful architecture
* HTTP Response Status Code
* Authentication and Autorization with JWT Bearer
* Swagger Config
* Create Middleware

        Global exception control in the application through the creation of a Middleware to have control of the message.
* OpenApi
* Flow control to avoid try catch and be more informative in the messages that are returned to the client - HTTP Response
* **Unit Test - XUnit - Moq** 🧪
    
        Eliminate dependencies through the Moq library to guarantee the logical flow of the application without external effects.

* Separation of responsibilities in the code and architecture
* **SOLID** Principles
* Implementation of Interfaces and Dependency Injection
* ORM > Entity Framework Core with SQLSERVER

        Constraints: PK, FK, NotNull, Unique, join associative table on two tables in many-to-many relationship.

        Eager Loading

        Transactions allow you to process multiple database operations atomically so code a transaction to perform an insert on two tables in a many-to-many relationship and preserve atomicity. EF maintains atomicity if the Save()/SaveAsync() method has not been executed before, but in my case I needed to perform Save on one table and then take its PK and insert to the other table and maintain atomicity between the two; RollBack is applied on failure.

* Repository Pattern
* Code First
* Asynchronous methods


## Warnings

* JWT. User data travels on each request, so you must use an SSL/HTTPS connection. Do not put sensitive data in User Claims because these fields are not encrypted. Anyone who has the token will be able to access the resources that are available to them.

        Remember, Try to delegate token issuance to a third party (IdentityServer4, Auth0, AD, etc), so you don't need to implement or maintain that part of the system and only need to consume/validate the generated tokens.

* Add CORS according to your own policy.

## Pending improvements

* Implement Unit Of Work.
* Define the type of primary key in the db with a GUID. It helps horizontal scalability because it allows sharding, it allows migrating to NoSQL and the code dependency is established in the application and not in the db. The DB would continue to comply with validating the data as unique. GUID also helps security because sequential numbers create vulnerability to the system.
* Add a minimum of 1 in the ID's through DataAnnotations so that ModelState prevents resources from being consumed unnecessarily. Good flow control must be created to avoid unnecessary consumption of resources in the database or expensive processes to execute
* Send the logs to a text file with a library like nlog.
* Increase unit tests
* Create extension method to separate dependency injection configuration; another to separate the connection configuration to the database.
* Handle exception with the Middleware of the framework: 'app.UseExceptionHandler("/error");'. Delete custom middleware that handle exception not controlled.

# Please go to Dropbox to see working pictures, diagrams and certificates. Thank you!

https://www.dropbox.com/scl/fo/5sjlajcgvdczqwm3g5n1f/h?dl=0&rlkey=lflx4jktws8tjw9tnqwaso5gj

# High level design

![My Remote Image](https://uc25c5b90c15861212af8228c208.previews.dropboxusercontent.com/p/thumb/ABtDptl2Y-AjYppuHSLLMg68MQS_s_F3SnALWeESKiGvHcJTWPB4MYH0NtgppuKW-VoyyKbNtG4ZpfXSdF04R2-V-zNe5sP7zALJc0yRtMM4bW1Vp6hxXLvTxA5SvgKi01sllAu2FENDj6q4Ec-CGxDqYuWP70sVQ3v2P0ChYNBZFD80RuuwMd6cpVsgVE_PPQy4AUyE0bmgCH5AwvtVV7eneSg-tZJnxubsV0TCEvbdqnbema5xRzwjdEIgPliaoSdjVxSvOa2Aiy0GxE34T7Wj0DitM1Tm_GpB2IWwwt8gD_e3g3f0ipUNVvdgSoGhHgjfhTzqKLQu9a5_FpeNyxhaQwkNQEvGvKk2_iaeLpS2Rx1-mE8AQDLshexnDAj1fCqzZmZjIK99BYTXzwNgTJbwy-kL3yG139LTzElLlSjcPQ/p.png)

# DB: Physical diagram
![My Remote Image](https://uc856d249a80f9043fa627059010.previews.dropboxusercontent.com/p/thumb/ABxEyv89E_K-uyMk1Guv15tMsPtKh-r-Z2NZNLJcTkig_v4bl-TrjKsBOJ9T30OFmlOwksujyyMbRHnIBKqJNULY12KSbSylbyxULUHJh2Fcq5Dn0lPbaz8NLkuWEn3_XLOYttyKUU09i787L4-QRNPqxzz5V6C2DT9VUFl_UXn94Lhq2XezKfOq931TSmE_Ku7P1_cUtkAjm2x_lbe8_Agh4Op_iWLcG7TSNFboD1QGHbRqCheIXAWnqVYzXGQnrVA0K3XnobHAn7yURtiq6FoBzQOzmhikcRiVyDqIiGbJg-FpbkjkksGGevNTivesJQtVI9rtnGhLory2_G3JcFnFZrjpN72oewjmj9t019b16GmdkfL16q9wZP7t8XxhmLmoMkhmoLH3D6LzU01v9aSDgufOzrWkY1MjFFjIRx76zA/p.png)

# Examples of API behavior
![My Remote Image](https://ucb43486a36034b915d58d6a95b7.previews.dropboxusercontent.com/p/thumb/AByFXRVMUf_RJLMV8SC_BrUfE0Mft5gN48g4L_q7-ZopwKHRcpVsSYb7nGBD5kGroKJq8-t6vnvTbRKy-BHiv2lOFQXiiTsWz4GLKs_QmblQJkOpmb8y6wWnu7CC4Ig4bWkAcQCjD9ASVtO9r-Qt1fmBNhxuEMu3zhe01tbC_S4k6spKqcuqJMGIN3qXSGBKMIoV4UNYQ5iwHoPHuJsCFvMfWUDAPH_ZmXUcoDDysKazJv7jBJSp34K7lMglk3fgtd6vpHLdKZpAom0si8KiBlt6CUsuycUkAI-kP-n__qC8W_wcs-8Pi2tDAeRwg3vaf17DVhIfnZyoMO2p4ZE_tz7zhDL9FX5efy665YndKfM9Hczck3Qbt4HfutgS5kS9zWTJANU5mommWpuprCex_ep7oaOfTkDSc0RUARQwM899xg/p.png)

![My Remote Image](https://uc1d03ffae143fa792537e09ee73.previews.dropboxusercontent.com/p/thumb/ABxihqYksvBQHX8bzDMsk63PFMOi7i6Z_DfLMWrymAF0gglWbgLELvmkts4HwNSmmHMKU31Qtn1VI3-RgGr7XBA_toJdRXs64jQyZ0_ZRtTDDQHv8tJz7ppoAbTklQckjLar0q7aW8AaXgeSfkyukQhCyvx5ZrBRkyxw0A8UcvB9gFJ6TtH-rbstAXXoWmyZCWeD-CKdeHCaOE7i2hFAf36Igd7NlrdlmRxmlAwhzjTSOYIa1EZUAC-5W-uzh1wzPknEmF9ENSA-5oaz3U-UvzjcStitj5ULK7WYz6lDsNr8jkMrIfUm0JRwZI9tyzNm5-ZkoNjOECMf-HtseGVHWIhuMOJ75aBeKv2ZqYzxJjc38l_daBYkaUd4zbd-kgp9qk5JMRaQzjRYqjeCaqmt9mUBveXR6fqDPmeDxXR508z5pQ/p.png)

![My Remote Image](https://uce607b900adcf96f5f2d9eb5de4.previews.dropboxusercontent.com/p/thumb/ABxSK_tg15kQC0Bfd6rK9AvUY0mCmtZN-7OmbesMJ8Hnpm1Kmr-3YaiGkubbO9tP1bIYN_SQwgf2BVfhKvGAQk6qqqvVO_5AhVR9h9bEdssEjMdojIK1yt4MBXrtaSRvb4a1tu4Q68sJDytIr9M-7bqpDjopfj4ICaGka_9mU-ZYxb66kPhDrcSEaXAZqUtxwCx5_yfMJl3ZvwmgbRX6mvhVpPEZYh7RD0vPAifKdhDq_al0gblal_bekWX0SzZTAZVsvStY5YXP85KbKryZS0vkoaSt_fLxzve-PqmRhXBJqcmdAzq9eX7OCCNlUJprCcaTPOkRgfaEQa-nbQ9MKBl6ZXsbYsR3qjO0rIiwQFXJP7CCOiMF8LWopYtGOKpH5hEADvsYgbl1NCu_6hkdwzzhjNrBBNTxchTgJ38LyWiT2A/p.png)

![My Remote Image](https://uc50f113cb8bc7784f6c8c16a800.previews.dropboxusercontent.com/p/thumb/ABxw_74muhvRAbRHkibSIGJn9JFTCGT2Q-jkkSS_g6JHvRA0VtTDk4rEcGY9ZLFd1INHdhOLb03sMSAhzS2BhNsplljvdKRFcY4A6XhCDd0kgxEWtV9m57A5eghqCQOGnuCk6B4V7kOGhbtNf-2fxKkQlqk5kjzbVkvOPSbtnDhLOvfFBw1x9AJVrldYsULs7ATr1-7BbF_-GN0y1PsAmTF57-LElTqXuxVkOwLvTXyKlfjf0-fqDXwOyY-Pv7tTAVrQ8-u9eI6swXpPoZFMYG3Dp2TuVrx0pJ4WVqRZAAhlRoE-iU6abfT6oMzadv6_PzqJDENukJ_V9SuLB5eVTWX2kK3LaZgZ3RV6Jv_S8UzRYOOoV-_Ny4NMeNHpfG4fbFNOqhhVtRSIjsG085fTgaHRJxhrpbf6s62qZBjFY96sfQ/p.png)

![My Remote Image](https://uce35e69d927742236e4d2aa68e8.previews.dropboxusercontent.com/p/thumb/ABxwkoW3Uw6IXYsZtKLHo3hdvVe72iHdthXcgEiJwlsytEZONeR7BdFYT1E-VjkaMaGx2SUG6tXXLR9YuJB0oKoFMoyT2S8iRXs2hqfo3qBbjuGa2BQC7gWS7DAZThrjjZF00HYBv5xcjUffkmar_8WNcEmWlAadYmg6kKM0bVmYohlPzghxBQZtITeGFD-Hg8kJrwJ-EBTkj5gidhGuEDdraoVMFy3v2HT-IuCfp59KZ9eqkfhmdhh9_F9AO-U6o6Qt57_qZL24bdC8wdmztSTH63mF-k0__b7SFIyHcWxCD4jq2LoyvZMqQ0zRfTtwUgwxPpPAJcWgJ6IjJvgwMJ8jv2jt1APq8th6jVbxFD_BMFgWnjWT1Q_SHnzIDl0l1srow3RX8vbdNo9Di1vg4ezayWd095g7WtmxgnNCr4YSZw/p.png)

# Courses
## Includes those taken in PersonalJournal

![APIS con .NET](https://ucb2f91ad882e440712094e281eb.previews.dropboxusercontent.com/p/pdf_img/ABx_PHpEVkKxTFniQnQMm32ipJM9yTr3CfjLNR6TwKCKnyDdy19bvMppSd4tO6WyxM0VvC-rjFYdKrSOfEGbBGBBXAliyR6TPmp6AK4Dzu4Sch4Zp9BtoLgODD8v5uyURyAg70Q06W-Gz2pLaHjVRMqBX3DxNJU1q6eHveqh2CupKocqvP6nhSkhI8-Qwv6ysYk6J47orQVgjHRJ2WQ5ukW2qHfblwhkwGyPACpftPjgB7KSN3DLT3BgWsWsa0TVJebU4lrxXNrBSLFdylFX4GaeS-gdmBHbF_cgFXtJS7ryvSU8psgmJpBjxuXQWi2YY1LcyhjdVtKmxalBsN7cl8WNcC5axhnVfuuMtugrJQ4hzT4X4eW4VOfmK43hARGpO_fcAPdCdLauxJsHJUd2YvYR/p.png?page=0&scale_percent=0)

![Arquitectura Backend](https://uc6fa32ac0c0b4733e01f66b8019.previews.dropboxusercontent.com/p/pdf_img/ABzKbWURDf8-i2RyQBiZ-jkAqNjSRJNXnFFf40I75bRtO4ZOUDrUnBQdALH2vfdfsKnYnkyCbaI1y6726ItXP6IsvoQuuNamASyo7ZoYX7aCzUy4qnNkQd4qarqGHUgBVJUCqEnsqHiw2jufM6S7JfCq6OkXmdKzC2i2hufBhPqMXRnwtfzbmGYhwZp8T2_YqQVkrIrNggCjyeSIv6xjrwTTmtLqHf1KOOuE5BMPg0TmCkuFLrvOjFVWnsQONyXpBpQJgxZnAdomBHG0BlL0YAd9NOMAWpuc6VCijGmEKQbUHULpU2iIlofJndg6ScPLMjZraPuYXK4D-wNexvrqPtQ3c1Q5l71E8827MdPR_mhIe5Z1r4O1m0iQf1ZkEESMcvQNSP-bIk0O5nNfaZiqzhph/p.png?page=0&scale_percent=0)

![Pruebas unitarias con XUnit](https://uc08323a4878da615e779361ce61.previews.dropboxusercontent.com/p/pdf_img/ABugBztNbUQOncEd2yEnDvV7Og-6O1dODQkQCnd7O-ThmCAvUuJBVUVmPbF0eSF9pkh-AAUgIBHCd8UfqC_gMFbpzdnI6-QglUyY5rO6_ygJmhN0p4cxzU1NY1XzW0C7x-Pv2amMHAKnY3WySQ92rNDVVKv7TDK8-tMB2nVDHynICclYIdhC2adroP0VWtHIRrGc5of_-1yGh05mEKU6fLSHscnyJ5YJWIMEuHRlgrUC2oStQK38ktfLcOCBa1VjXZf0q_0USQvMFKwq9h5eHByfw65CVrhlkEOlJEA0af0UWKwDkY1Pxr6MA4VYdRq4cUbUkgO1WZ5LqgBkAyiPPSEyJIJv10PO1Gd2eC1zpneyJri7QVsz9V8MH1wYYsIvi_YpdoKHhRJ3zGiuRbvyVIvR/p.png?page=0&scale_percent=0)

![Fundamentos de pruebas de software](https://uc84a382b7965c8594b1be54e558.previews.dropboxusercontent.com/p/pdf_img/AByHYrHNCKOZtpe1FiwjZqD6eY4-ECegQqD8KPUD5Fnl0qIpduAuY0t9nXgcbGUHCoQjlkkz7zRe02NpqpfvBYDhrvPY9RwzaDeRtHkBTRaGIc4z11srOr6Q2KMzZTisil0CsmHwX99QI7ZlIjHVtiOisibq4hReA22ujobv8A4EUCLce1q_kxphSkpYmx41kFN1cFMpAGGjZCgV_bynd_F_c5j1Bz1ocQCQB9Tza0KJ71Pn10t7TpJBjw5OcSmjiqKBnIazUeAqyAk7p-wJ_qtuGAP8AiHnatN01JdZ9rPLmGVpW0ypXt50jQxZa3tWTStotYz12LqvWWPIZ7yTKfm934qBptViw9jBHmPnht2qcapnKCx6UP3N5yFhsJrO_SMUQkxpEWubr9lilozpC4W9/p.png?page=0&scale_percent=0)