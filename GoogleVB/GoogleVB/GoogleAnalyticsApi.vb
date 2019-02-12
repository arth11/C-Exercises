Imports System.IO
Imports System.Net
Imports System.Text

Public Class GoogleAnalyticsApi
    Private Shared Sub Main(ByVal args As String())
        'Dim item As GoogleAnalyticsItem = New GoogleAnalyticsItem() With {
        '    .AdSenseId_a = "1",
        '    .ItemCode_ic = "5430001111"
        '}
        'Track(item)

        Dim team As New List(Of Staff)

        team.Add(New Staff With {.ID = 101, .Office = "London", .Department = "Admin", .Salary = 24000})
        team.Add(New Staff With {.ID = 103, .Office = "London", .Department = "Admin", .Salary = 23500})
        team.Add(New Staff With {.ID = 104, .Office = "Leeds", .Department = "Admin", .Salary = 23500})
        team.Add(New Staff With {.ID = 107, .Office = "London", .Department = "Accounts", .Salary = 22900})
        team.Add(New Staff With {.ID = 109, .Office = "Leeds", .Department = "Accounts", .Salary = 22700})
        team.Add(New Staff With {.ID = 112, .Office = "Leeds", .Department = "Admin", .Salary = 22650})
        team.Add(New Staff With {.ID = 113, .Office = "London", .Department = "Admin", .Salary = 22000})
        team.Add(New Staff With {.ID = 120, .Office = "London", .Department = "Admin", .Salary = 22000})



        Dim summary1 = From member In team
                       Group member By keys = New With {Key member.Office, Key member.Department}
                       Into Group
                       Select New With {.office = keys.Office, .dept = keys.Department,
                                        .sum = Group.Sum(Function(x) x.Salary),
                                        .max = Group.Max(Function(x) x.Salary)}

        For Each item In summary1
            Console.WriteLine("{0} {1} {2} {3}", item.office, item.dept,
                              item.sum.ToString("c0"), item.max.ToString("c0"))
        Next

    End Sub

    Private Shared Sub Track(ByVal item As GoogleAnalyticsItem)
        If String.IsNullOrEmpty(item.AdSenseId_a) Then item.AdSenseId_a = "1998914617"
        If String.IsNullOrEmpty(item.ItemCode_ic) Then item.ItemCode_ic = "empty"
        Dim request = CType(WebRequest.Create("https://www.google-analytics.com/collect"), HttpWebRequest)
        request.Method = "POST"
        Dim postData = New Dictionary(Of String, String) From {
            {"v", "1"},
            {"tid", "UA-7225725-62"},
            {"cid", "555"},
            {"t", item.AdSenseId_a},
            {"ec", item.ItemCode_ic}
        }
        Dim postDataString = postData.Aggregate("", Function(data, [next]) String.Format("{0}&{1}={2}", data, [next].Key, [next].Value)).TrimEnd("&"c)
        request.ContentLength = Encoding.UTF8.GetByteCount(postDataString)

        Using writer = New StreamWriter(request.GetRequestStream())
            writer.Write(postDataString)
        End Using

        Try
            Dim webResponse = CType(request.GetResponse(), HttpWebResponse)

            If webResponse.StatusCode <> HttpStatusCode.OK Then
                Throw New Exception("Google Analytics tracking did not return OK 200")
            End If

        Catch ex As Exception
        End Try
    End Sub
End Class

