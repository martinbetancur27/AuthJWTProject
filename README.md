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

# High level design

![My Remote Image](https://uc25c5b90c15861212af8228c208.previews.dropboxusercontent.com/p/thumb/ABtDptl2Y-AjYppuHSLLMg68MQS_s_F3SnALWeESKiGvHcJTWPB4MYH0NtgppuKW-VoyyKbNtG4ZpfXSdF04R2-V-zNe5sP7zALJc0yRtMM4bW1Vp6hxXLvTxA5SvgKi01sllAu2FENDj6q4Ec-CGxDqYuWP70sVQ3v2P0ChYNBZFD80RuuwMd6cpVsgVE_PPQy4AUyE0bmgCH5AwvtVV7eneSg-tZJnxubsV0TCEvbdqnbema5xRzwjdEIgPliaoSdjVxSvOa2Aiy0GxE34T7Wj0DitM1Tm_GpB2IWwwt8gD_e3g3f0ipUNVvdgSoGhHgjfhTzqKLQu9a5_FpeNyxhaQwkNQEvGvKk2_iaeLpS2Rx1-mE8AQDLshexnDAj1fCqzZmZjIK99BYTXzwNgTJbwy-kL3yG139LTzElLlSjcPQ/p.png)

# DB: Physical diagram
![My Remote Image](https://uc856d249a80f9043fa627059010.previews.dropboxusercontent.com/p/thumb/ABtzM1bV-pbzS6tI3b7Kr1zi8u74oA6ewU6tc9E8sz3Aaiuk3l2McbHmeN8BG8LVqzBsQtwO07gBlzwaL34eJ8N6Xz4yLC4tcaIu0-V-IvlNQluY4vzDsBYWjMAM1ActSTfBQaNxuePeZfmhPpdEycPHmCyJAQRUc_LouVruhw0G4G1BGqeKnFkTXWPIzP0gBkB4IoyV1TsB4nx9S9Q3jh7m9zs-tqZr1tWCZy6EwgjO1V72RgUvlIu6F09_pHoIVI-D-ERKlMkofIBb80BvijW1QvODn7muO4RhHgNxL4lwBWKDCXUjSwn_fx6rEi62rO2yGrNJitq136Loib8PMsE6V1oS1HwTCJ2CqJGCx510mdIr-pCoLAlblRtZUB6BdnJMVGttfeU_wCEaayZU_NxyW7qiog0NXMi_3lDuhVXd9A/p.png)

# Examples of API behavior
![My Remote Image](https://ucb43486a36034b915d58d6a95b7.previews.dropboxusercontent.com/p/thumb/ABvDrrkR9E5-PXeROlyxwM5PhHknS4cMi4KjOuh80K0sCEYHHPXKZ_200yOzE7TDM3GQigq8b2PfZSqvc4PL1jcDlHQugre-2H5H9yKM-_pzAMgk2_P_FaFMhmxmkMTSB1BOzxLjtd6-02Znrw5F6dIW-LJspqV2_njBlvivluLDcvLXTyJtBSgkL4SoFFByIPjMr3i2n7TBkNElaOj_FXdbcY45xgOIikTEA_K8v-xbDflpCWskok55fKC4_O3Occ6GOXMYSdZNNJU6TwhpyOcFcOI3VarxlDoHcL4LsZTAoEZRKWwFSZ7rj-avJUofqPdCYXaFEbvWwgyQH_RurVCpIfiqP-5SNsFtWLf1cYwceZUv627KeeD3IQkiMipYuDnkOWrfwad1PZeWgegiJw01e28a6OZ4of56xS4CXXajGQ/p.png)

![My Remote Image](https://uc1d03ffae143fa792537e09ee73.previews.dropboxusercontent.com/p/thumb/ABsDioWCukzCz_lsP14nIhMTWm2IDiakGOvn0eBGFnCLYB8Veo8_KTCC7ygYdq7Sexu9sFSgilL-MvqwA_f8Ycota0f74xW146HWzidBmx7E8PafpAfpeSRHlsdDIi-KdSjAuv8XWbXvXqseBc-MNjeBNsOjfLd9YOkq1JqyeA7m9frzLAgvNVQW87ektkWzkuYtjTdCfn6U8A8zBuZ2CgXCT3813RWqc1fNs5HWH9nzZIe4qsIlfWi1umqRdlv2mnzTGkw_U4436gV5vlMiQQNgAmbl9q0LvfzxT6j7sjzhmQr-RE0sm1Ps3RT3G2eZwdBa7NTmbJ0R1E2yyFB4Se5QoQZpLGPTke7Zu2U-g6AXlUxVzVlFeyoq9NnCTBgC7b1XYNHydJYJNZtcQt2HPmhNFUmib0NHk7nYydVqhNSgzA/p.png)

![My Remote Image](https://uc8f5e738cf48ed8decd451dcdc0.previews.dropboxusercontent.com/p/thumb/ABvG4cOig13Lw8-W2zHE2bnVRtgd2rbMctYonql0VZmdKdKGksVx_7tjTv0XuNqAMV3qV4N51AVkSRi3T-luKoO6kd8bHAJxwP4CWsNbiDMVilc6YuG-H8jqi0LPTiZxu5WJcRbzBjJbiP3yjLEpoqmQPy_yogAkrPjsU-nTmn80EHPHCGVTaSsAgZT2U6-NlnaZv3oRZFomw73fX5NZww-2nZvV5RpJNnm-HtmJh03usz8jxuqGNdUHUcg7jqdWik_s0f6j92V_pxk_fTAkmoyyo98tIRH8W-3iFhg8wZdmEmFKEtS62imHBLrYarnng6sC1Ki53UHq5YOIBVQbEp7lIdcJeaao6iNYHwjbZXRoE7eokTcqJhk3gT_TVD4hd7Sx1-NqGh3T5eZY-RBhmE6mfjctnwjPY52E_-r1-Goadg/p.png)

![My Remote Image](https://uce607b900adcf96f5f2d9eb5de4.previews.dropboxusercontent.com/p/thumb/ABumHAZt8JWj0rJrg5RtBudZEbWmBeMoovW6g48ZTfP3QPJgdGhMGw6rQjSBO3lK5eNRhSpThiCwwGim45KkhVVRr337Lb65uyPANVdvt0Mlh2NfRCSHAcMOE01ETPZ9yGVjulIjjWgoXJz35RTZG3KQDOL2xFp9SN9kECMauItsZF3Qm0_1NW8Gk0LZwp2IAKvKqlqLzfaZl5g2sQXZ1pbvfiApwX82ghxEBqKrqUZ04lIt1eMGBP-3rmjn0fcWgQwatiZPvxX_ILVpqvfGRiVDaIQD2qhtWnMQShop9OitwVyb8Rl2UqcRdE2-Vxw44Aib4b_FHjNzm_m56ijknkgIEbZqx3gc3lkHT5cSSvpBNVzPnYh5hfBJgK2fOud7duaLYIlHty5B5UieEsGV_1BO-iajqZr1O1pmXgCVigRYmQ/p.png)

![My Remote Image](https://uc50f113cb8bc7784f6c8c16a800.previews.dropboxusercontent.com/p/thumb/ABuB1bg3MIfTntCVjWw2ubQvtvfBSudT-DJXcTOnHKufMcU923Fpr3wGcPZqcGDEcILrYTVNk27L1xBHiuF4wfZxfZOcmQwk828tYJJYrZDQqKoktL12mCQLCURhyw-lQxopVU8tWSKLwc0nl0C9G2uBjQRZOoEzNH8KM5eNL4r6j0kkWrOUl5dIw5V04tzt3JxYaxbfvqcRKKHL8hFzTzDUfrmv3OTOsuovDbiYAZ43us7aJKv-jay5BedScFQq4D0rYbeFnod3C3uJggKl4vllgTsw8CeJbdXUHMAuSqjki0N-xjUsic3z5QEGDQiEhVhCM4kAyE1Iv0ymSiYrrGmlXExe3VBp-lsE90GeIb6eVPeztkltRAzJW4DSdlPlgvJq1OkpQ9L5EgsrnbTVTmDbGnGeuoj42f_zEZ-4m66LVg/p.png)

![My Remote Image](https://uc7d1cee4ddf178dd57d5d2831d5.previews.dropboxusercontent.com/p/thumb/ABui8i1ezvzlRNduz8PeXwDoftJEQy8_FMs-kBWt0Fi8v_y3VJ33iwoslOG8C6J3m9Kf-AY-dGaJIlpixIZtENszjRLp1MY3m5aGuuHjhX4tca21FeMg6R4rvFo_uaXHEC_qZcpIrCNfRnLP9fAJCFF74Mkj0K0mAcVEHAqUeSolTn9SSOVhE0lrFIx5usASRl5yOhAx-QRG41ZQbb008lFLzDVgVLG1-Wb8zOKcaYYzlpvnEzeAb-LVtv6_IBXqcCHmi6YApxVmaF-MeNOUvh0f5NACHDDjA78CeKvs89MjaZAblhRLsimP_kz1WQ4t3YcpV-x-zjcnuOQvtHXOQn9ddgsdvjfNcqQeBYdOcq5pwQh6UQdQdJ6F5AOIEegjEfqRyTh7ZA25G_FN8zl4TAxn8pw8W12MnvWf2Nl48xsT2w/p.png)

![My Remote Image](https://uce35e69d927742236e4d2aa68e8.previews.dropboxusercontent.com/p/thumb/ABvNaVhkto01jVZ_dLXPFjbaqBzdWTv88cI-uCbvrNFa8QwMwqyvYusbgE17eaCE9ohNLSYfbYF5ji6SXrv_0d1VIkuam_MMcIpczO1VFHuhAUgBl11Lo3zjZGLAx-LKdT4lHIFDW3jFi-Tai3rZfqLk0XlXZBHXluuH2ycOR40Rb04kQ4LjIXJSbYAPe8A3ha2REUVIJXLFjld1mXXZYTgluA9InK2aE4cfDdw8wbEjGlC9LYUZOIBfvcYUgFBxcyh1sPxII1O8qtlyJOOx62WE-KXb77_ip-npmaEDf7eNsEZhQd-K3AhYQJMHt8CGf0zR16Y6oUa35CkVtE7bHutJKmm3mtUvOqN1nODJ9bBl_X8NXboTrYs4khCBAbmtI9L9jomvGwZZABPXvrQXMOy13dfwXPtQVK9DB79xq6Ts2w/p.png)

# Courses
## Includes those taken in PersonalJournal

![APIS con .NEt](https://ucb2f91ad882e440712094e281eb.previews.dropboxusercontent.com/p/pdf_img/ABvZRviTGOZeSMXSflK4ZtIHNXshfIUerPlMwkVyRAz2mhzcKHKC2ylPWMtww-kIdAmimnRicBLa8spvrzYLXx7iMKTKxAhBFBa2_yrtBe51r6VNcDlgJ6oei0-e_QXwjnCRPjBC_h3IQUFvQ_LZ99zju9S7h1umRRMe_2BV73t8FiPpwua91A5DldpFz45MgRlV4DM_8iEto9wgSofJb7Ttq5AXle35WUnh0YSN4G17SPeZgayl4mGT7Rql1aiCIMunbKwyTVTfE3hhEYzlp18QWzc5-3uOwlRUkt9_zRChc8cTC2kyCAza-V4MfA0_ty6B9CHV5uZJu67ZkzT0EYwZuUMXq1UUHjlsoIn7OgWf6efHu4cGtNpdTNhLvv5jW2b7rrdU3RGwNpLqx8saZRIc/p.png?page=0&scale_percent=0)

![Arquitectura Backend](https://uc6fa32ac0c0b4733e01f66b8019.previews.dropboxusercontent.com/p/pdf_img/ABseRUcJzSVfV1044fwNhYHg3ydNyewIC-tDnhtfqyFrJB5yLURherVWPHf4dLpM6YuY3aW6jPLFPUziaCa3WF0Y8UWwabuPkbn9Y7NeS-0501FSpI2sSXjSlPCjnFBdLAVRPiHiJHEdMmGSqKOn71wfbAYFDDu159R1RiCYpwMS5xmPO7QpLxwj6QjtHeCqcD2SErEwhEdZBYMFYcAl-RNYrGkuBoxvQ-gEg0dN8t9ExLfo22o46qMgs96xyLAqXWNZAF62JFz5TMvJUyE-ZtH2rNe-2gbJ4VHx40t1AGD6HyohFXWhV0G4ORLbN5FNRJK8vjGDM-WPDzEFmzIGFta2vFliFvKTfhLQp3qHeTHvY9NqzWva-2siZ23PjELsVqtd7DWzPF5KrETunTtj_rBP/p.png?page=0&scale_percent=0)

![Pruebas unitarias con XUnit](https://uc08323a4878da615e779361ce61.previews.dropboxusercontent.com/p/pdf_img/ABugBztNbUQOncEd2yEnDvV7Og-6O1dODQkQCnd7O-ThmCAvUuJBVUVmPbF0eSF9pkh-AAUgIBHCd8UfqC_gMFbpzdnI6-QglUyY5rO6_ygJmhN0p4cxzU1NY1XzW0C7x-Pv2amMHAKnY3WySQ92rNDVVKv7TDK8-tMB2nVDHynICclYIdhC2adroP0VWtHIRrGc5of_-1yGh05mEKU6fLSHscnyJ5YJWIMEuHRlgrUC2oStQK38ktfLcOCBa1VjXZf0q_0USQvMFKwq9h5eHByfw65CVrhlkEOlJEA0af0UWKwDkY1Pxr6MA4VYdRq4cUbUkgO1WZ5LqgBkAyiPPSEyJIJv10PO1Gd2eC1zpneyJri7QVsz9V8MH1wYYsIvi_YpdoKHhRJ3zGiuRbvyVIvR/p.png?page=0&scale_percent=0)

![Fundamentos de pruebas de software](https://uc84a382b7965c8594b1be54e558.previews.dropboxusercontent.com/p/pdf_img/ABt2B6zgu3OR3VXStsoj8UHXcXGgblFgt0V_SXYZWqVCe1BbN0bZpDLhJmEBKGz17iaMAk6GDoklBTFu9mDold09gt-JHlmCmHWzOSMTR1p6AgBudWxxHYx0cQZFUD8j_6739mhW8Gou85W9illZIZZUNMAVhSS2t9C1vZ2IqVsPCcdfB0coEdvoKa2P8WEsEYIAIVwNeSJIN6cDjloGVBPY7meXL3nigwuULnx41VR-cgFG3qpGjsnNuPoSnWvM-O8gpEqujXBs5f7N8EF4qbMYrdINP8aI5-HXO3SMu79knwcrc6EVAZYLlJkh50gp8t5hPVs4SrFFvAYwA8bz8moCdL324HJcdU6UGM0qexXTsvkmvNAJMnpyK98KtGkAzi-4AXB993P3y4b2EHjmt1kd/p.png?page=0&scale_percent=0)