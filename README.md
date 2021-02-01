# lucit-layout-drive-csharp

Example
```c#
   // 1. Initialize Client
   var apiToken = "XXX";
   var client = new LayoutDriveClient(apiToken);
   
   // 2. Get Creative
   var exportId = "lch-4C9D";
   var locationId = "SC_MH_1";
   
   var result = await client.GetCreativeAsync(exportId, locationId);
   
   // 3. Submit Play (ping back) 
   var playRequest = new SubmitPlayRequest
   {
      PlayDateTime = DateTime.UtcNow,
      Duration = TimeSpan.FromHours(1),
      DigitalBoardId = 19302,
      CreativeId = "C1-4C9D-LP-4V4Y"
   };
            
   await client.SubmitPlayAsync(playRequest);
   
   // 4. Validate item hash
   var isValidHash = await client.ValidateItemHashAsync(creative);
```
