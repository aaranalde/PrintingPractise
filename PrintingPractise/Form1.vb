
Imports System.Drawing.Printing

Public Class Form1

    Dim StoreName As String = "SERBA ADA STORE"
    Dim StoreAddress As String = "Jl. Kehidupan No. 100"
    'Dim Img As Image = Image.FromFile("d:\logo.jpg") '--> for fixed location
    Dim Img As Image = Image.FromFile(Application.StartupPath() & "\logo.jpg")
    Dim TransNo As String = "TCN10-20191204-001"
    Dim TransDate As String = Format(Now, "yyyy-MM-dd HH:mm:ss")

    'for item sales | untuk item penjualan
    Dim dtItem As DataTable
    Dim arrWidth() As Integer
    Dim arrFormat() As StringFormat

    'declaring printing format class
    Dim c As New PrintingFormat

    'for subtotal & qty total
    Dim dblSubtotal As Double = 0
    Dim dblQty As Double = 0
    Dim dblPayment As Double = 50000

    Sub Data_Load()
        dtItem = New DataTable
        With dtItem.Columns
            .Add("itemname", Type.GetType("System.String"))
            .Add("qty", Type.GetType("System.String"))
            .Add("price", Type.GetType("System.String"))
        End With

        Dim ItemRow As DataRow

        ItemRow = dtItem.NewRow()
        ItemRow("itemname") = "Taro Snack"
        ItemRow("qty") = "1"
        ItemRow("price") = "5000"
        dtItem.Rows.Add(ItemRow)

        ItemRow = dtItem.NewRow()
        ItemRow("itemname") = "Kopi Ice"
        ItemRow("qty") = "2"
        ItemRow("price") = "7000"
        dtItem.Rows.Add(ItemRow)

        ItemRow = dtItem.NewRow()
        ItemRow("itemname") = "Lolipop"
        ItemRow("qty") = "5"
        ItemRow("price") = "1000"
        dtItem.Rows.Add(ItemRow)

    End Sub


    Private Sub ButtonImprimir_Click(sender As Object, e As EventArgs) Handles ButtonImprimir.Click

        Data_Load()

        Printer.NewPrint()

        Printer.Print(Img, 200, 100)

        'Setting Font
        Printer.SetFont("Courier New", 11, FontStyle.Bold)
        Printer.Print(StoreName) 'Store Name

        'Setting Font
        Printer.SetFont("Courier New", 8, FontStyle.Regular)
        Printer.Print(StoreAddress & ";", {280}, 0) 'Store Address

        Printer.Print(" ") 'spacing

        Printer.Print(TransNo) ' Transaction No
        Printer.Print(TransDate) ' Trans Date

        Printer.Print(" ") 'spacing
        Printer.SetFont("Courier New", 8, FontStyle.Bold) 'Setting Font
        arrWidth = {90, 40, 50, 70} 'array for column width
        arrFormat = {c.MidLeft, c.MidRight, c.MidRight, c.MidRight} 'array alignment 

        'column header split by ;
        Printer.Print("item;qty;price;subtotal", arrWidth, arrFormat)
        Printer.SetFont("Courier New", 8, FontStyle.Regular) 'Setting Font
        Printer.Print("------------------------------------") 'line

        dblSubtotal = 0
        dblQty = 0
        'looping item sales | loop item penjualan
        For r = 0 To dtItem.Rows.Count - 1
            Printer.Print(dtItem.Rows(r).Item("itemname") & ";" & dtItem.Rows(r).Item("qty") & ";" &
                          dtItem.Rows(r).Item("price") & ";" &
                          (dtItem.Rows(r).Item("qty") * dtItem.Rows(r).Item("price")), arrWidth, arrFormat)
            dblQty = dblQty + CSng(dtItem.Rows(r).Item("qty"))
            dblSubtotal = dblSubtotal + (dtItem.Rows(r).Item("qty") * dtItem.Rows(r).Item("price"))
        Next

        Printer.Print("------------------------------------")
        arrWidth = {130, 120} 'array for column width | array untuk lebar kolom
        arrFormat = {c.MidLeft, c.MidRight} 'array alignment 

        Printer.Print("Total;" & dblSubtotal, arrWidth, arrFormat)
        Printer.Print("Payment;" & dblPayment, arrWidth, arrFormat)
        Printer.Print("------------------------------------")
        Printer.Print("Change;" & dblPayment - dblSubtotal, arrWidth, arrFormat)
        Printer.Print(" ")
        Printer.Print("Item Qty;" & dblQty, arrWidth, arrFormat)

        'Release the job for actual printing
        'Printer.DoPrint() 'default printer
        Printer.DoPrint(ComboBoxPrinter.Text) ' selected printer

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim prtdoc As New PrintDocument()
        Dim strDefaultPrinter As String = prtdoc.PrinterSettings.PrinterName
        For Each strPrinter As String In PrinterSettings.InstalledPrinters
            ComboBoxPrinter.Items.Add(strPrinter)
            If strPrinter = strDefaultPrinter Then
                ComboBoxPrinter.SelectedIndex = ComboBoxPrinter.Items.IndexOf(strPrinter)
            End If
        Next
    End Sub
End Class

