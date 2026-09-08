# Introduction 

SPFx project for Sharepoint list extension which launches PA Canvas form instead of default SP form. 
Since Sharepoint out of box customization with Canvas App is not ALM supprotive and works in a smaller window, its not possible to build robust application UI using SharePoint's own out of box Canvas form.

All settings are controlled by a local list named as PAURLSettings: unless lists are registered here - list extension will not work for any list. 
This PAURLSettings contains list guid, 3 PA urls for 3 different scenarios  (New, Edit, View).

# Getting Started

This is SPFx list extension 

# Build and Test
https://learn.microsoft.com/en-us/sharepoint/dev/spfx/extensions/get-started/building-simple-cmdset-with-dialog-api
https://blog.alfredmut.com/post/2023/10/27/spfx-extensions-listview-command-set

# Contribute

Developed by : Suman Pal