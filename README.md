# NeoWZ

Maplestory WZ serializer



## Feature

* Package reader
* Image serialize / deserialize
* Customizable serializer
* Customizable wz object



## Note

Default sound/canvas object only provide **raw** data currently.
It is necessary that build you own converter to convert between your audio/image to sound/canvas raw data.



## Requirements

* .NET 6.0



## Usage

* Serializer

  ```C#
  using NeoWZ.Serialize;
  
  var serializer = ComSerializer.Default;
  serializer.
  ```

  