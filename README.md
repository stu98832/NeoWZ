# NeoWZ

可以讀取 Maplestory .wz / .img 檔案的函式庫

改良自原本的舊專案 [libwz](https://github.com/stu98832/libwz)



## 功能

* .wz 檔案讀取
* .img 檔案序列化/反序列化
* 自訂 image 檔案解析
* 自訂可序列化物件



## 注意

預設的 Sound 與 Canvas 物件只提供 raw data
因此需要自行撰寫資料解析的部份



## 需求

* .NET 6.0



## 使用方式

* 反序列化

  ```C#
  using NeoWZ.Serialize;
  using NeoWZ.Serialize.Property;
  
  var serializer = ComSerializer.Default;
  var property = serializer.Deserialize<WzProperty>("file.img");
  ```
  
* 序列化

  ```C#
  using NeoWZ.Serialize;
  using NeoWZ.Serialize.Property;
  
  var serializer = ComSerializer.Default;
  var property = new WzProperty();
  property.Add(new WzInt("integer", 1));
  serializer.Serialize("file.img", property);
  ```

* wz 讀取

  ```C#
  using NeoWZ;
  
  var package = WzPackage.Open("package.wz");
  var dir = package["path/to/directory"].To<WzDirectory>();
  var file = package["path/to/file"].To<WzArchive>();
  ```

* wz 串 image serializer

  ```C#
  using NeoWZ;
  using NeoWZ.Serialize;
  using NeoWZ.Serialize.Property;
  
  
  var package = WzPackage.Open("package.wz");
  var file = package["file.img"].To<WzArchive>();
  var stream = file.Stream;
  stream.Seek(0, SeekOrigin.Begin);
  
  var serializer = ComSerializer.Default;
  var property = serializer.Deserialize<WzProperty>(stream);
  ```

  