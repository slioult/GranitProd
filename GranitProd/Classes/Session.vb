Imports MySql.Data.MySqlClient
Imports System.Data

Public Class Session

#Region "Fields"

    Private _Identifier As Long
    Private _Login As String
    Private _Password As String
    Private _IsAddCmd As Boolean
    Private _IsUpdCmd As Boolean
    Private _IsDelCmd As Boolean
    Private _IsDispCA As Boolean
    Private _IsDispPanel As Boolean
    Private _IsUpdConfig As Boolean
    Private _IsUpdSession As Boolean

#End Region

#Region "Properties"

    Public Property Identifier As Long
        Get
            Return Me._Identifier
        End Get

        Set(ByVal value As Long)
            Me._Identifier = value
        End Set
    End Property


    Public Property Login As String
        Get
            Return _Login
        End Get
        Set(ByVal value As String)
            Me._Login = value
        End Set
    End Property

    Public Property Password As String
        Get
            Return Me._Password
        End Get

        Set(ByVal value As String)
            Me._Password = value
        End Set
    End Property

    Public Property IsAddCmd As Boolean
        Get
            Return _IsAddCmd
        End Get
        Set(ByVal value As Boolean)
            Me._IsAddCmd = value
        End Set
    End Property

    Public Property IsUpdCmd As Boolean
        Get
            Return _IsUpdCmd
        End Get
        Set(ByVal value As Boolean)
            Me._IsUpdCmd = value
        End Set
    End Property

    Public Property IsDelCmd As Boolean
        Get
            Return _IsDelCmd
        End Get
        Set(ByVal value As Boolean)
            Me._IsDelCmd = value
        End Set
    End Property

    Public Property IsDispCA As Boolean
        Get
            Return _IsDispCA
        End Get
        Set(ByVal value As Boolean)
            Me._IsDispCA = value
        End Set
    End Property

    Public Property IsDispPanel As Boolean
        Get
            Return _IsDispPanel
        End Get
        Set(ByVal value As Boolean)
            Me._IsDispPanel = value
        End Set
    End Property

    Public Property IsUpdConfig As Boolean
        Get
            Return _IsUpdConfig
        End Get
        Set(ByVal value As Boolean)
            Me._IsUpdConfig = value
        End Set
    End Property

    Public Property IsUpdSession As Boolean
        Get
            Return _IsUpdSession
        End Get
        Set(ByVal value As Boolean)
            Me._IsUpdSession = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

        Me.Login = String.Empty
        Me.Password = String.Empty
        Me.IsAddCmd = False
        Me.IsUpdCmd = False
        Me.IsDelCmd = False
        Me.IsDispCA = False
        Me.IsDispPanel = False
        Me.IsUpdConfig = False
        Me.IsUpdSession = False
        Me.Identifier = 0

    End Sub


    Public Sub New(ByVal login As String, ByVal password As String, Optional ByVal isAddCmd As Boolean = False, Optional ByVal isUpdCmd As Boolean = False, Optional ByVal isDelCmd As Boolean = False,
                   Optional ByVal isDispCA As Boolean = False, Optional ByVal isDispPanel As Boolean = False, Optional ByVal isUpdConfig As Boolean = False, Optional ByVal isUpdSession As Boolean = False,
                   Optional ByVal identifier As Long = 0)
        Me.Login = login
        Me.Password = password
        Me.IsAddCmd = isAddCmd
        Me.IsUpdCmd = isUpdCmd
        Me.IsDelCmd = isDelCmd
        Me.IsDispCA = isDispCA
        Me.IsDispPanel = isDispPanel
        Me.IsUpdConfig = isUpdConfig
        Me.IsUpdSession = isUpdSession
        Me.Identifier = identifier
    End Sub


#End Region

#Region "DataAccess"

    ''' <summary>
    ''' Permet de récupérer les informations d'une session dans la base de données
    ''' </summary>
    ''' <param name="isAllRights">Permet de ne pas modifier les droit de la session Administrateur</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSession(Optional ByVal isAllRights As Boolean = False) As Session
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))
        Dim rights As String = String.Empty
        If Not isAllRights Then rights = " AND Login <> 'Administrateur'"

        Try
            connection.Open()

            Dim parIdentifierSession As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdentifierSession)

            Objects = connection.ExecuteQuery("SELECT Identifier, Login, Password, IsAddCmd, IsUpdCmd, IsDelCmd, IsDispCA, IsDispPanel, IsUpdConfig, IsUpdSession FROM Session WHERE Identifier=@Identifier" + rights, parameters)

            parameters = Nothing

            connection.Close()

            For Each obj In Objects
                Me.Login = obj(1).ToString()
                Me.Password = obj(2).ToString()
                Me.IsAddCmd = Boolean.Parse(obj(3))
                Me.IsUpdCmd = Boolean.Parse(obj(4))
                Me.IsDelCmd = Boolean.Parse(obj(5))
                Me.IsDispCA = Boolean.Parse(obj(6))
                Me.IsDispPanel = Boolean.Parse(obj(7))
                Me.IsUpdConfig = Boolean.Parse(obj(8))
                Me.IsUpdSession = Boolean.Parse(obj(9))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me
    End Function

    ''' <summary>
    ''' Permet de récupérer toutes les sessions dans la base de données
    ''' </summary>
    ''' <param name="isAllRights">Permet de ne pas modifier les droit de la session Administrateur</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetSessions(Optional ByVal isAllRights As Boolean = False) As List(Of Session)
        Dim sessions As New List(Of Session)
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim Objects As New List(Of List(Of Object))
        Dim rights As String = String.Empty
        If Not isAllRights Then rights = " WHERE Login <> 'Administrateur'"

        Try
            connection.Open()

            Objects = connection.ExecuteQuery("SELECT Identifier, Login, Password, IsAddCmd, IsUpdCmd, IsDelCmd, IsDispCA, IsDispPanel, IsUpdConfig, IsUpdSession FROM Session" + rights)

            connection.Close()

            For Each obj In Objects
                sessions.Add(New Session(obj(1).ToString(), obj(2).ToString(), Boolean.Parse(obj(3)), Boolean.Parse(obj(4)), Boolean.Parse(obj(5)), Boolean.Parse(obj(6)), Boolean.Parse(obj(7)), Boolean.Parse(obj(8)), Boolean.Parse(obj(9)),
                                         Long.Parse(obj(0))))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return sessions
    End Function

    ''' <summary>
    ''' Permet d'insérer une session en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Insert() As Long
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)
        Dim Objects As New List(Of List(Of Object))

        Try
            connection.Open()

            Dim parLogin As MySqlParameter = connection.Create("@Login", DbType.String, Me.Login)
            parameters.Add(parLogin)
            Dim parPassword As MySqlParameter = connection.Create("@Password", DbType.String, Me.Password)
            parameters.Add(parPassword)
            Dim parIsAddCmd As MySqlParameter = connection.Create("@IsAddCmd", DbType.Boolean, Me.IsAddCmd)
            parameters.Add(parIsAddCmd)
            Dim parIsUpdCmd As MySqlParameter = connection.Create("@IsUpdCmd", DbType.Boolean, Me.IsUpdCmd)
            parameters.Add(parIsUpdCmd)
            Dim parIsDelCmd As MySqlParameter = connection.Create("@IsDelCmd", DbType.Boolean, Me.IsDelCmd)
            parameters.Add(parIsDelCmd)
            Dim parIsDispCA As MySqlParameter = connection.Create("@IsDispCA", DbType.Boolean, Me.IsDispCA)
            parameters.Add(parIsDispCA)
            Dim parIsDispPanel As MySqlParameter = connection.Create("@IsDispPanel", DbType.Boolean, Me.IsDispPanel)
            parameters.Add(parIsDispPanel)
            Dim parIsUpdConfig As MySqlParameter = connection.Create("@IsUpdConfig", DbType.Boolean, Me.IsUpdConfig)
            parameters.Add(parIsUpdConfig)
            Dim parIsUpdSession As MySqlParameter = connection.Create("@IsUpdSession", DbType.Boolean, Me.IsUpdSession)
            parameters.Add(parIsUpdSession)

            Dim query As String = "INSERT INTO Session (Login, Password, IsAddCmd, IsUpdCmd, IsDelCmd, IsDispCA, IsDispPanel, IsUpdConfig, IsUpdSession) VALUES (@Login, MD5(@Password), @IsAddCmd, @IsUpdCmd, @IsDelCmd, @IsDispCA, @IsDispPanel, @IsUpdConfig, @IsUpdSession)"

            connection.ExecuteNonQuery(query, parameters)

            Objects = connection.ExecuteQuery("SELECT Max(Identifier) FROM Session")

            For Each obj In Objects
                Me.Identifier = Long.Parse(obj(0))
            Next

            parameters = Nothing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

        Return Me.Identifier
    End Function

    ''' <summary>
    ''' Permet de mettre à jour une session en base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Update()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdSession As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdSession)
            Dim parLogin As MySqlParameter = connection.Create("@Login", DbType.String, Me.Login)
            parameters.Add(parLogin)
            Dim parPassword As MySqlParameter = connection.Create("@Password", DbType.String, Me.Password)
            parameters.Add(parPassword)
            Dim parIsAddCmd As MySqlParameter = connection.Create("@IsAddCmd", DbType.Boolean, Me.IsAddCmd)
            parameters.Add(parIsAddCmd)
            Dim parIsUpdCmd As MySqlParameter = connection.Create("@IsUpdCmd", DbType.Boolean, Me.IsUpdCmd)
            parameters.Add(parIsUpdCmd)
            Dim parIsDelCmd As MySqlParameter = connection.Create("@IsDelCmd", DbType.Boolean, Me.IsDelCmd)
            parameters.Add(parIsDelCmd)
            Dim parIsDispCA As MySqlParameter = connection.Create("@IsDispCA", DbType.Boolean, Me.IsDispCA)
            parameters.Add(parIsDispCA)
            Dim parIsDispPanel As MySqlParameter = connection.Create("@IsDispPanel", DbType.Boolean, Me.IsDispPanel)
            parameters.Add(parIsDispPanel)
            Dim parIsUpdConfig As MySqlParameter = connection.Create("@IsUpdConfig", DbType.Boolean, Me.IsUpdConfig)
            parameters.Add(parIsUpdConfig)
            Dim parIsUpdSession As MySqlParameter = connection.Create("@IsUpdSession", DbType.Boolean, Me.IsUpdSession)
            parameters.Add(parIsUpdSession)

            Dim query As String = "UPDATE Session SET Login = @Login, IsAddCmd = @IsAddCmd, IsUpdCmd = @IsUpdCmd, IsDelCmd = @IsDelCmd, IsDispCA = @IsDispCA, IsDispPanel = @IsDispPanel, IsUpdConfig = @IsUpdConfig, IsUpdSession = @IsUpdSession WHERE Identifier = @Identifier"

            connection.ExecuteNonQuery(query, parameters)

            parameters = Nothing

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try

    End Sub

    ''' <summary>
    ''' Permet de supprimer une session de la base de données
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Delete()
        Dim connection As New MGranitDALcsharp.MGConnection(My.Settings.DBSource)
        Dim parameters As New List(Of MySqlParameter)

        Try
            connection.Open()

            Dim parIdSession As MySqlParameter = connection.Create("@Identifier", DbType.Int32, Me.Identifier)
            parameters.Add(parIdSession)

            connection.ExecuteNonQuery("DELETE FROM Session WHERE Identifier=@Identifier", parameters)

            parameters.Clear()

            connection.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error")
        Finally
            Try
                connection.Close()
            Catch ex As Exception
            End Try
        End Try
    End Sub

#End Region

End Class
