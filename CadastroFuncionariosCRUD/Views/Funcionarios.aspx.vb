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

    'Metodo para carregar a GridView do arquivo Pessoas.aspx, com os dados inseridos na tebela do Banco
    Public Sub CarregaGrid()
        Dim query As String = "SELECT * FROM Funcionarios"
        Dim dt As New DataTable
        btnAtualizar.Visible = False
        btnCancelar.Visible = False
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
        'Passar campos vazios para toda vez que add/editar/cancelar, limpe os campos text
        txtNome.Text = ""
        txtDataNasc.Text = ""
        txtEmail.Text = ""
        txtTelefone.Text = ""
    End Sub

    'Metodo que cria uma nova pessoa ao preencher os campos Text e clicar no botão "Adicionar"
    Public Sub CreatePeople()
        pessoas.Nome = txtNome.Text.Trim()
        pessoas.DataNasc = DateTime.Parse(txtDataNasc.Text)
        pessoas.Email = txtEmail.Text.Trim()
        pessoas.Telefone = txtTelefone.Text.Trim()

        Dim query As String = "INSERT INTO Funcionarios(nome, dataNascimento, email, telefone) VALUES (@Nome, @Data, @Email, @Telefone)"
        Using connection As New SqlConnection(connectionString)
            Dim command As New SqlCommand(query, connection)
            command.Parameters.AddWithValue("@Nome", pessoas.Nome)
            command.Parameters.AddWithValue("@Data", pessoas.DataNasc)
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

    'Carrega os dados do banco para os campos text, quando é clicado no botão "Editar"
    Public Sub LoadDataEdit(ByVal id As Integer)
        Dim queryNome As String = "SELECT *FROM Funcionarios where ID = @id"
        Using connection As New SqlConnection(connectionString)
            Dim command As New SqlCommand(queryNome, connection)
            command.Parameters.AddWithValue("@id", id)
            Try
                connection.Open()
                Dim reader As SqlDataReader = command.ExecuteReader()
                Dim lTextBox As New List(Of TextBox) From {txtNome, txtDataNasc, txtEmail, txtTelefone}
                If reader.Read = True Then
                    For Each textBox As TextBox In lTextBox
                        textBox.Text = reader(textBox.ID.Replace("txt", "").ToString)
                    Next
                End If
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End Using
    End Sub

    Protected Sub Btn_Editar(sender As Object, e As EventArgs)
        btnAdicionar.Visible = False
        btnAtualizar.Visible = True
        btnCancelar.Visible = True
        pessoas.Id = Integer.Parse(CType(sender, Button).CommandArgument)
        LoadDataEdit(pessoas.Id)
        Session("IdEditar") = pessoas.Id
    End Sub

    Protected Sub UpdatePeople(id As Integer)
        id = Convert.ToInt32(Session("IdEditar"))
        pessoas.Nome = txtNome.Text.Trim()
        pessoas.DataNasc = DateTime.Parse(txtDataNasc.Text)
        pessoas.Email = txtEmail.Text.Trim()
        pessoas.Telefone = txtTelefone.Text.Trim()
        Dim queryUpdate As String = "UPDATE Funcionarios SET NOME = @Nome, DATA = @Data, EMAIL = @Email, TELEFONE = @Telefone WHERE ID = @ID"
        Using connection As New SqlConnection(connectionString)
            Try
                connection.Open()
                Dim commandUpdate As New SqlCommand(queryUpdate, connection)
                commandUpdate.Parameters.AddWithValue("@ID", id)
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

    Private Sub BtnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        btnAdicionar.Visible = True
        CarregaGrid()
    End Sub

    Private Sub btnAtualizar_Click(sender As Object, e As EventArgs) Handles btnAtualizar.Click
        UpdatePeople(pessoas.Id)
        Session.Remove("IdEditar")
        btnAdicionar.Visible = True
        CarregaGrid()
    End Sub

End Class