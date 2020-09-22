# Yale C# to control your Smart Yale Alarm
Yale Smart Alarm Client **c#**

**Usage**

```
  var yAlarm = YaleAlarm.Instance;
  if(yAlarm.GetAuthenticated())
  {
    
     var arStatus = yAlarm.AlarmStatus();

     if (yAlarm.AlarmStatus().Equals("disarm"))
     {
          yAlarm.ArmDevice("arm");
     }
     else
     {
          yAlarm.ArmDevice("disarm");
     }
  } 
            
```
