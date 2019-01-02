[更新紀錄]

  2018-10-18, App啟用測試中 by Jep


[安裝與使用]

  使用工作排程器安排固定時間執行此應用程式

  或手動執行此應用程式

  針對料號為 "F%" 及 非 "F0%"選擇，若將條件有調整之需要，需調整 DataBase Trigger。


[事件紀錄文件]

  完成程序後會生成，成功或例外事件紀錄

  事件紀錄以「每月份」檔案來製作 log紀錄，並寫入至同應用程式中的 EventLog資料夾內。


[指令]
  
  taskschd.msc (工作排程器)


[設計相關追溯]

  {正式區DataBase}
    PLM, Trigger: [Innovator].[PART]
    PLM, OperatorTable: [innovator].[PART_BARCODE_MATERIALDATA]
    SAIBARCODE, Table寫入: [dbo].[MaterialData]

  {測試用環境}
    SAEE_QAS, Trigger: [dbo].[PART_TEST]
    SAEE_QAS, OperatorTable: [dbo].[PART_TEST_OP]
    SAEE_QAS, Table寫入: [dbo].[PART_MaterialData]
