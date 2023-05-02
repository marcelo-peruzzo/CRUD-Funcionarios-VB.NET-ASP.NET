Imports System.Data.SqlClient


Public Class Funcionarios
    Inherits System.Web.UI.Page

    'Instanciando a conexão do banco, recebendo os dados da ConnectionStrings no arquivo Web.config
    Private Shared ReadOnly connectionString As String = ConfigurationManager.ConnectionStrings("BDCrudVb").ConnectionString
    'Instanciando um novo objeto Pessoas, da classe Pessoas.vb com seus metodos
    Dim pessoas As New Pessoa()
    Dim updateId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CarregaGrid()
            If Session("showAlertAdd") IsNot Nothing AndAlso Session("showAlertAdd").ToString() = "True" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SweetAlert", "showSuccessAlert();", True)
                Session.Remove("showAlertAdd")
            End If

            If Session("showAlertDelete") IsNot Nothing AndAlso Session("showAlertDelete").ToString() = "True" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SweetAlert", "showDeletedSuccessAlert();", True)
                Session.Remove("showAlertDelete")
            End If

            If Session("showAlertEdit") IsNot Nothing AndAlso Session("showAlertEdit").ToString() = "True" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "SweetAlert", "showEditSuccessAlert();", True)
                Session.Remove("showAlertEdit")
            End If
        End If
    End Sub

    'Metodo para carregar a GridView do arquivo Funcionarios.aspx, com os dados inseridos na tebela do Banco
    Public Sub CarregaGrid()
        Dim query As String = "SELECT * FROM Funcionarios"
        Dim dt As New DataTable
        Using connection As New SqlConnection(connectionString)
            Dim command As New SqlCommand(query, connection)
            Dim adapter As New SqlDataAdapter(command)
            Try
                connection.Open()
                adapter.Fill(dt)

                grd.DataSource = dt
                grd.DataBind()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        End Using

    End Sub

    'Metodo que cria uma nova pessoa ao preencher os campos Text e clicar no botão "Adicionar"
    Public Sub CreatePeople()
        pessoas.Nome = txtNome.Text.Trim()
        pessoas.DataNasc = txtDataNasc.Text
        pessoas.Email = txtEmail.Text.Trim()
        pessoas.Telefone = txtTelefone.Text.Trim()

        Dim query As String = "INSERT INTO Funcionarios(nome, dataNascimento, email, telefone) VALUES (@Nome, @Data, @Email, @Telefone)"
        Using connection As New SqlConnection(connectionString)
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@Nome", pessoas.Nome)
            command.Parameters.AddWithValue("@Data", pessoas.DataNasc.Date)
            command.Parameters.AddWithValue("@Email", pessoas.Email)
            command.Parameters.AddWithValue("@Telefone", pessoas.Telefone)
            Try
                connection.Open()
                Dim lineAfected = command.ExecuteNonQuery
                If lineAfected > 0 Then
                    Session("showAlertAdd") = True
                    Response.Redirect(Request.Url.AbsolutePath)
                    Exit Sub
                End If

            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        End Using
    End Sub

    'Metodo que trata o Evento do botão "Adicionar" e aciona o metodo "CreatePeople()"
    'para gerar uma nova pessoa, e chama o metodo CarregaGrid() para listar o novos dados na GridView
    Private Sub ButtonGridAdicionar_Click(sender As Object, e As EventArgs) Handles btnAdicionar.Click
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "abrirModal", "$('#modalCadastro').modal('show');", True)
    End Sub

    Private Sub ButtonModal_Click(sender As Object, e As EventArgs) Handles ButtonAdd.Click
        CreatePeople()
    End Sub

    'método é responsável por abrir a modal e preencher os campos de entrada na modal com os dados da linha editada na GridView
    Protected Sub Btn_Editar(sender As Object, e As EventArgs)
        Dim btn As Button = CType(sender, Button) 'a função CType é usada para converter o objeto SENDER(que disparou o evento do btn_editar) em um objeto do tipo button, permitindo acesso as props e metodos do onjeto Button
        Dim row As GridViewRow = CType(btn.NamingContainer, GridViewRow) 'a propriedade NamingContainer do objeto btn, obtem uma referencia do container pai do botão (neste caso, a linha da GridView que contém o botão) e o CType converte o objeto NamingContainer e um objeto do tipo GridViewRow
        Dim rowIndex As Integer = row.RowIndex 'variavel para obter o indice da linha clicada na gridview
        'cada prorpiedade do objeto pessoas, recebe o indice da linha da gridview qual foi cliacada para editar.
        pessoas.Nome = grd.Rows(rowIndex).Cells(0).Text
        pessoas.DataNasc = grd.Rows(rowIndex).Cells(1).Text
        pessoas.Email = grd.Rows(rowIndex).Cells(2).Text
        pessoas.Telefone = grd.Rows(rowIndex).Cells(3).Text
        'script jQuery para definir os valores dos campos de entrada na modal com os dados da linha selecionada e abrir a modal.
        Dim script As String = "$('#MainContent_txtNome').val('" & pessoas.Nome & "');"
        script &= "$('#MainContent_txtDataNasc').val('" & Date.Parse(pessoas.DataNasc).ToString("yyyy-MM-dd") & "');"
        script &= "$('#MainContent_txtEmail').val('" & pessoas.Email & "');"
        script &= "$('#MainContent_txtTelefone').val('" & pessoas.Telefone & "');"
        script &= "$('#MainContent_buttonAdd').val('Atualizar');"
        script &= "$('#modalCadastro').modal('show');" 'abrir a modal com o click do btn editar

        'método estático RegisterClientScriptBlock da classe ScriptManager para registrar o script jQuery na página. Isso fará com que o script seja executado pelo navegador quando a página for renderizada preenchendo os campos da modal.
        ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "abrirModal", script, True)
    End Sub


    Public Sub UpdatePeople(ByVal id As Integer)
        pessoas.Nome = txtNome.Text.Trim()
        pessoas.DataNasc = DateTime.Parse(txtDataNasc.Text)
        pessoas.Email = txtEmail.Text.Trim()
        pessoas.Telefone = txtTelefone.Text.Trim()
        Dim queryUpdate As String = "UPDATE Funcionarios SET nome = @Nome, dataNascimento = @Data, email = @Email, telefone = @Telefone WHERE ID = @ID"
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim commandUpdate As New SqlCommand(queryUpdate, connection)
                commandUpdate.Parameters.AddWithValue("@Nome", pessoas.Nome)
                commandUpdate.Parameters.AddWithValue("@Data", pessoas.DataNasc)
                commandUpdate.Parameters.AddWithValue("@Email", pessoas.Email)
                commandUpdate.Parameters.AddWithValue("@Telefone", pessoas.Telefone)
                Dim lineAfected = commandUpdate.ExecuteNonQuery
                If lineAfected > 0 Then
                    Session("showAlertEdit") = True
                    Response.Redirect(Request.Url.AbsolutePath)
                End If
            Catch ex As Exception

            End Try
        End Using
    End Sub

    Protected Sub BtnDeletar_Click(sender As Object, e As EventArgs)
        'A função CType converte o objeto sender em um objeto Button.
        'A função Integer.Parse converte o valor do parâmetro CommandArgument, que é uma string, em um inteiro.
        Dim pessoas As New Pessoa()
        pessoas.Id = Integer.Parse(CType(sender, Button).CommandArgument)

        DeletePeople(pessoas.Id)
        CarregaGrid()

    End Sub

    Public Sub DeletePeople(ByVal id As Integer)
        Dim query As String = "DELETE FROM Funcionarios WHERE ID = @id"
        Using connection As New SqlConnection(connectionString)
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@id", id)
            Try
                connection.Open()
                Dim lineAfectedDell = command.ExecuteNonQuery()
                If lineAfectedDell > 0 Then
                    Session("showAlertDelete") = True
                    Response.Redirect(Request.Url.AbsolutePath)
                    Exit Sub
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        End Using
    End Sub

End Class