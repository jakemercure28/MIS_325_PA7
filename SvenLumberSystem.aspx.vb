Imports System.Data
Imports System.Data.SqlClient
'SQL client for use of SQL enabled commands
Partial Class _Default
    Inherits System.Web.UI.Page

    'database connection to MFO3jake.mercure
    Public Shared Con As New SqlConnection("Data Source=cb-ot-devst04.ad.wsu.edu;Initial Catalog=MF03jake.mercure;Persist Security Info=True;User ID=jake.mercure;Password=a19152a9")

    'global data table varibles to be filled with data from server, stored on webpage.
    Public Shared deliveryTable As New DataTable
    Public Shared truckerTable As New DataTable
    Public Shared sawyerTable As New DataTable
    Public Shared lumberTable As New DataTable

    'get each item in deliveries table on server
    Public Shared deliveryCategoriesAdapter As New SqlDataAdapter("SELECT * FROM Deliveries", Con)
    Public Shared deliveryCategoriesCommand As New SqlCommandBuilder(deliveryCategoriesAdapter)

    Public Shared truckerNamesAdapter As New SqlDataAdapter("SELECT * from Truckers", Con)
    Public Shared sawyerNamesAdapter As New SqlDataAdapter("SELECT * from Sawyers", Con)
    Public Shared LumberTypesAdapter As New SqlDataAdapter("SELECT * from LumberTotals", Con)

    'webpage initialization procedure
    Private Sub _Default_Init(sender As Object, e As EventArgs) Handles Me.Init

        'exit sub if datatables contain collumns, if they are already initialized.
        If truckerTable.Columns.Count > 0 Then Exit Sub
        If sawyerTable.Columns.Count > 0 Then Exit Sub
        If lumberTable.Columns.Count > 0 Then Exit Sub

        'upload server data from previous deliveries to webpage
        Try
            deliveryCategoriesAdapter.Fill(deliveryTable)
            GridView1.DataSource = deliveryTable
            GridView1.DataBind()

            deliveryCategoriesAdapter.FillSchema(deliveryTable, SchemaType.Mapped)

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        'get each trucker name and ID from the trucker data set in server. Fill trucker data table on webpage.

        Try
            truckerNamesAdapter.Fill(truckerTable)
            With DropDownList1
                .DataSource = truckerTable
                .DataTextField = "TruckerName"
                .DataValueField = "TruckerID"       'primary key
                .DataBind()
            End With
            ' catch error if trucker names cant upload to drop down list
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        ' get each sawyer name and ID from saywer data set in server. Fill sawyer data table.
        Try
            sawyerNamesAdapter.Fill(sawyerTable)
            With DropDownList2
                .DataSource = sawyerTable       'fill drop down list 2 on web page with each sawyer name and ID
                .DataTextField = "SawyerName"
                .DataValueField = "SawyerID"    'primary key
                .DataBind()
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try

        ' get each lumber type and price per ton from server.
        Try
            LumberTypesAdapter.Fill(lumberTable)    'fill lumber table with lumber data from server
            With DropDownList3                      'fill drop down list with lumber table data.
                .DataSource = lumberTable
                .DataTextField = "LumberType"       'lumber type is a string
                .DataValueField = "LumberID"        'lumber ID is primary key
                .DataBind()
            End With
        Catch ex As Exception
            Response.Write(ex.Message)
        End Try


        'bind trucker and saywer data tables to the coresponding gridviews on each multiview on webpage.
        GridView2.DataSource = truckerTable
        GridView2.DataBind()
        GridView3.DataSource = sawyerTable
        GridView3.DataBind()
        GridView4.DataSource = lumberTable
        GridView4.DataBind()

    End Sub

    'multiview switching procedures, connected to link buttons on top of webpage
#Region "mulitview index"
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        MultiView1.ActiveViewIndex = 1
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        MultiView1.ActiveViewIndex = 2
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        MultiView1.ActiveViewIndex = 3
    End Sub

#End Region

    'create new record button procedure
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        'data row variable creates new row in delivery table, the table is displayed on webpage via gridview
        Dim dr As DataRow = deliveryTable.NewRow()

        'get each delivery number from delivery data table in server
        Dim deliveryCount As New SqlDataAdapter("SELECT DeliveryNumber from Deliveries", Con)


        'set each collumn in data row to corresponding collumn name in delivery table located on the server
        Try
            dr("DeliveryNumber") = 1                    'not functioning corectly, delivery number shouldnt need to exist on this webpage to function
            dr("DeliveryDate") = Convert.ToDateTime(TextBox1.Text)  'user entered date data
            dr("LumberID") = DropDownList3.SelectedValue            'user selected lumber ID data
            dr("DeliveryWeight") = TextBox2.Text                    'user entered tonnage data from textbox
            dr("TruckerID") = DropDownList1.SelectedValue           'user selected trucker
            dr("SawyerID") = DropDownList2.SelectedValue            'user selected sawyer
        Catch ex As Exception
            Response.Write(ex)
            Exit Sub
        End Try

        'check if user selected VIP on webpage
        If (CheckBox1.Checked = True) Then
            dr("VIPTrucker") = True
        Else
            dr("VIPTrucker") = False
        End If

        'calculate price per ton. This would be better if connected to sql database directly.
        Dim pricePerTon = DropDownList3.SelectedValue
        Select Case DropDownList3.SelectedValue
            Case 12839
                pricePerTon = 400
            Case 12840
                pricePerTon = 375
            Case 12841
                pricePerTon = 175
        End Select


        'calculate payout based on user entered data on webpage
        dr("TruckerPayout") = 0.3 * pricePerTon * TextBox2.Text
        dr("SawyerPayout") = 0.7 * pricePerTon * TextBox2.Text

        If (dr("VIPTrucker") = True) Then
            dr("TruckerPayout") = 1.01 * dr("TruckerPayout") 'VIP bonus factor
        End If

        'add row to the delivery table on webpage
        deliveryTable.Rows.Add(dr)

        'bind delivery table to gridview 1 on view 1 of multi view
        GridView1.DataSource = deliveryTable
        GridView1.DataBind()

        'send updated data table to server via SQL command
        deliveryCategoriesAdapter.Update(deliveryTable)

        'update individual trucker on trucker data table stored on the server.
        Dim updateTruckerCommand As New SqlCommand("UPDATE Truckers set NumberDeliveries += 1, TotalPayouts += @d1, TotalTonsDelivered += @d2, LastDelivery = @d3 WHERE TruckerID = @d4", Con)

        'parameters:
        'decimal Data containing dollar amount of individual trucker payout
        'decimal data containing user entered tonnage amount
        'int data containg user selected trucker ID from drop down list.
        With updateTruckerCommand.Parameters
            .Clear()
            .AddWithValue("@d1", dr("TruckerPayout"))
            .AddWithValue("@d2", TextBox2.Text)
            .AddWithValue("@d3", TextBox1.Text)
            .AddWithValue("@d4", DropDownList1.SelectedValue)
        End With

        'update individual sawyer on sawyer data table stored on the server.
        Dim updateSawyerCommand As New SqlCommand("UPDATE Sawyers set NumberDeliveries += 1, TotalPayouts += @d1, TotalTonsDelivered += @d2, LastDelivery = @d3 WHERE SawyerID = @d4", Con)

        With updateSawyerCommand.Parameters
            .Clear()
            .AddWithValue("@d1", dr("SawyerPayout"))
            .AddWithValue("@d2", TextBox2.Text)
            .AddWithValue("@d3", TextBox1.Text)
            .AddWithValue("@d4", DropDownList2.SelectedValue)
        End With

        'update lumber data on lumber totals data table stored on the server.
        Dim updateLumberTotalsCommand As New SqlCommand("UPDATE LumberTotals set NumberDeliveries += 1, TotalTons += @p1, LastDelivery = @p2 WHERE LumberID = @p3", Con)

        With updateLumberTotalsCommand.Parameters
            .Clear()
            .AddWithValue("@p1", TextBox2.Text)
            .AddWithValue("@p2", TextBox1.Text)
            .AddWithValue("@p3", DropDownList3.SelectedValue)
        End With

        're-establish connection to database if connection state is closed.
        If (Con.State = ConnectionState.Closed) Then
            Con.Open()
        End If

        'update the date sets
        updateTruckerCommand.ExecuteNonQuery()
        updateSawyerCommand.ExecuteNonQuery()
        updateLumberTotalsCommand.ExecuteNonQuery()

        'update the date tables on the webpage
        truckerTable.Clear()
        truckerNamesAdapter.Fill(truckerTable)
        GridView2.DataSource = truckerTable
        GridView2.DataBind()

        sawyerTable.Clear()
        sawyerNamesAdapter.Fill(sawyerTable)
        GridView3.DataSource = sawyerTable
        GridView3.DataBind()

        lumberTable.Clear()
        LumberTypesAdapter.Fill(lumberTable)
        GridView4.DataSource = lumberTable
        GridView4.DataBind()


    End Sub


End Class
