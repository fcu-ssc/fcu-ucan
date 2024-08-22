# FCU-UCAN

## 部署

1. 請先修改 `appsettings.json`
2. 執行 `dotnet publish -c Release -r win-x64`
3. 修改 `web.config`
4. 修改 `Default.db` 的寫入權限，內容 => 安全性 => 編輯 => 新增 => 進階 => 立即尋找 => `IIS_IUSERS` => 勾選修改權限

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <!-- 加入以下內容 -->
    <httpProtocol>
      <customHeaders>
        <remove name="X-Frame-Options" />
      </customHeaders>
    </httpProtocol>
  </system.webServer>
</configuration>
```

## 常用 libman 指令

| 說明 | 指令 |
| --- | --- |
| 還原程式庫檔 | `libman restore` |
| 刪除程式庫檔 | `libman clean` |

## 常用 efcore 指令

| 說明 | 指令 |
| --- | --- |
| 新增移轉 | `dotnet ef migrations add {移轉名稱} -o Data/Migrations` |
| 套用移轉 | `dotnet ef database update` |

## 使用套件

| 名稱 | 說明 |
| --- | --- |
| [toptal/gitignore.io](https://github.com/toptal/gitignore.io) | 產生 `.gitignore` |
| [alexkaratarakis/gitattributes](https://github.com/alexkaratarakis/gitattributes) | 產生 `.gitattributes` |
| [dotnet/aspnetcore](https://github.com/dotnet/aspnetcore) | framework |
