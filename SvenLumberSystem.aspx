<%@ Page Language="VB" AutoEventWireup="false" CodeFile="SvenLumberSystem.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
        .auto-style3 {
            width: 186px;
        }
        .auto-style4 {
            font-size: xx-large;
        }
        .auto-style5 {
            text-align: center;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style5">
            <strong><span class="auto-style4">Sven&#39;s Lumber Yard System</span></strong><br />
            <br />
            <asp:LinkButton ID="LinkButton1" runat="server">Data Input</asp:LinkButton>
&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton2" runat="server">List of Truckers</asp:LinkButton>
&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton3" runat="server">List of Sawyers</asp:LinkButton>
&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="LinkButton4" runat="server">Lumber Totals</asp:LinkButton>
        </div>
        <asp:MultiView ID="MultiView1" runat="server">
            <asp:View ID="View1" runat="server">
                &nbsp;&nbsp;&nbsp;
                <table class="auto-style1">
                    <tr>
                        <td class="auto-style3">Select Date</td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Select Trucker</td>
                        <td>
                            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                            <asp:CheckBox ID="CheckBox1" runat="server" Text="VIP?" />
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Select Sawyer</td>
                        <td>
                            <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Select Lumber Type</td>
                        <td>
                            <asp:DropDownList ID="DropDownList3" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">Enter Delivery Weight (Tons)</td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style3">
                            <asp:Button ID="Button1" runat="server" Text="Add Record" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:GridView ID="GridView2" runat="server">
                </asp:GridView>
                <br />
                <br />
            </asp:View>
            <asp:View ID="View3" runat="server">
                <asp:GridView ID="GridView3" runat="server">
                </asp:GridView>
                <br />
                <br />
            </asp:View>
            <asp:View ID="View4" runat="server">
                <asp:GridView ID="GridView4" runat="server">
                </asp:GridView>
                <br />
                <br />
            </asp:View>
        </asp:MultiView>
    </form>
</body>
</html>
