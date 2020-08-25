# lucit-layout-drive-csharp

Example
```c#
   var apiToken = "XXX";
   var client = new LayoutDriveClient(apiToken);
   
   
   var exportId = "lch-4C9D";
   var locationId = "SC_MH_1";
   
   var result = await client.GetCreativeAsync(exportId, locationId);
   
   
   var playRequest = new SubmitPlayRequest
   {
      PlayDateTime = DateTime.UtcNow,
      Duration = TimeSpan.FromHours(1),
      DigitalBoardId = 19302,
      CreativeId = "C1-4C9D-LP-4V4Y"
   };
            
   await client.SubmitPlayAsync(playRequest);
```
