
Namespace Properties


    ' このクラスでは設定クラスでの特定のイベントを処理することができます:
    '  SettingChanging イベントは、設定値が変更される前に発生します。
    '  PropertyChanged イベントは、設定値が変更された後に発生します。
    '  SettingsLoaded イベントは、設定値が読み込まれた後に発生します。
    '  SettingsSaving イベントは、設定値が保存される前に発生します。
    Partial Friend NotInheritable Class Settings
        Public Sub New()
            ' // 設定の保存と変更のイベント ハンドラーを追加するには、以下の行のコメントを解除します:
            '
            ' this.SettingChanging += this.SettingChangingEventHandler;
            '
            ' this.SettingsSaving += this.SettingsSavingEventHandler;
            '
        End Sub

        Private Sub SettingChangingEventHandler(ByVal sender As Object, ByVal e As System.Configuration.SettingChangingEventArgs)
            ' SettingChangingEvent イベントを処理するコードをここに追加してください。
        End Sub

        Private Sub SettingsSavingEventHandler(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            ' SettingsSaving イベントを処理するコードをここに追加してください。        
        End Sub
    End Class
End Namespace

