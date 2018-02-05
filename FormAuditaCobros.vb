Public Class FormAuditaCobros

    Private Sub MostrarDatos()
        Try


            Dim TotalCobrosContables As Double


            If IsNumeric(Me.mCobrosContabilidad) Then
                Me.TextBox1.Text = Format(Me.mCobrosContabilidad, "N")
            End If

            If IsNumeric(mAnticiposfacturados) Then
                Me.TextBox2.Text = Format(mAnticiposfacturados, "N")
            End If


            If IsNumeric(Me.mNotasCreditoNewGolf) Then
                Me.TextBox3.Text = Format(Me.mNotasCreditoNewGolf, "N")
            End If
            If IsNumeric(Me.mFacturasAnuladasNewGolf) Then
                Me.TextBox4.Text = Format(Me.mFacturasAnuladasNewGolf, "N")
            End If
            If IsNumeric(Me.mPagosCuentaBonos) Then
                Me.TextBox5.Text = Format(Me.mPagosCuentaBonos, "N")
            End If

            If IsNumeric(Me.mCobrosNewHotel) Then
                Me.TextBox6.Text = Format(Me.mCobrosNewHotel, "N")
            End If



            If IsNumeric(Me.mNotasCreditoNewGolfAnuladas) Then
                Me.TextBox10.Text = Format(Me.mNotasCreditoNewGolfAnuladas, "N")
            End If

            If IsNumeric(Me.mAnticiposrecibidos) Then
                Me.TextBox11.Text = Format(Me.mAnticiposrecibidos, "N")
            End If


            If IsNumeric(Me.mDtosFinancieros) Then
                Me.TextBox12.Text = Format(Me.mDtosFinancieros, "N")
            End If

            'Me.TextBox1.Text = Me.mCobrosContabilidad
            'Me.TextBox2.Text = Me.mAnticiposfacturados
            'Me.TextBox3.Text = Me.mNotasCreditoNewGolf
            'Me.TextBox4.Text = Me.mFacturasAnuladasNewGolf
            'Me.TextBox5.Text = Me.mPagosCuentaBonos
            'Me.TextBox6.Text = Me.mCobrosNewHotel


            TotalCobrosContables = Me.mCobrosContabilidad + Me.mDtosFinancieros + Me.mAnticiposrecibidos - Me.mAnticiposfacturados - Me.mNotasCreditoNewGolf - Me.mFacturasAnuladasNewGolf + Me.mPagosCuentaBonos + Me.mNotasCreditoNewGolfAnuladas
            Me.TextBox7.Text = Format(TotalCobrosContables, "N")

            Me.TextBox8.Text = Format(Me.mCobrosNewHotel, "N")
            Me.TextBox9.Text = Format(Me.mCobrosNewHotel - TotalCobrosContables, "N")

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub FormAuditaCobros_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Me.MostrarDatos()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ButtonImprimir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonImprimir.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.CheckBoxAnulados.Checked = True Then
                REPORT_SELECTION_FORMULA = "{VN_RECAU.MOVI_DATR}=DATETIME(" & Format(Me.mFecha, REPORT_DATE_FORMAT) & ")"
            Else
                REPORT_SELECTION_FORMULA = "{VN_RECAU.MOVI_DATR}=DATETIME(" & Format(Me.mFecha, REPORT_DATE_FORMAT) & ") AND {VN_RECAU.MOVI_ANUL}='0' AND {VN_RECAU.MOVI_CORR}='0' "
            End If

            Dim Form As New FormVisorCrystal("RecauNC por factura.RPT", Me.mNombreHotel & " Auditoría de Cobros", REPORT_SELECTION_FORMULA, Me.mStrCentral, Me.mStrHotel, True, False)
            Form.MdiParent = Me.MdiParent
            Form.StartPosition = FormStartPosition.CenterScreen
            Form.Show()
            Me.Cursor = Cursors.Default

        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message, MsgBoxStyle.Information, "Atención")


        End Try
    End Sub

    Private Sub ButtonReparaView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonReparaView.Click
        Try

            Dim DB As New C_DATOS.C_DatosOledb(Me.mStrHotel)

            Dim SQL As String

            SQL = "CREATE OR REPLACE VIEW VN_RECAU (MOVI_ORIG, "
            SQL += "                                 MOVI_DATR, "
            SQL += "                                 RESE_CODI, "
            SQL += "                                 RESE_ANCI, "
            SQL += "                                 ALOJ_CODI, "
            SQL += "                                 UNMO_CODI, "
            SQL += "                                 FORE_CODI, "
            SQL += "                                 MOVI_CORR, "
            SQL += "                                 MOVI_ANUL, "
            SQL += "                                 MOVI_TIMO, "
            SQL += "                                 FACT_CODI, "
            SQL += "                                 SEFA_CODI, "
            SQL += "                                 TIRE_CODI, "
            SQL += "                                 MOVI_VDEB, "
            SQL += "                                 MOVI_AUTO, "
            SQL += "                                 FORE_DESC, "
            SQL += "                                 MOVI_TITU, "
            SQL += "                                 PAPO_CODI, "
            SQL += "                                 MOVI_CODI, "
            SQL += "                                 MOVI_DARE, "
            SQL += "                                 FACT_EXTE, "
            SQL += "                                 SEFA_EXTE,MOVI_NUDO "
            SQL += "                                ) "
            SQL += "AS "
            SQL += "   SELECT   'NH' MOVI_ORIG, TNHT_MOVI.MOVI_DATR, TNHT_MOVI.RESE_CODI, "
            SQL += "            TNHT_MOVI.RESE_ANCI, TNHT_MOVI.ALOJ_CODI, TNHT_MOVI.UNMO_CODI, "
            SQL += "            TNHT_MOVI.FORE_CODI, TNHT_MOVI.MOVI_CORR, TNHT_MOVI.MOVI_ANUL, "
            SQL += "            TNHT_MOVI.MOVI_TIMO, TNHT_MOVI.FACT_CODI, TNHT_MOVI.SEFA_CODI, "
            SQL += "            TNHT_MOVI.TIRE_CODI, TNHT_MOVI.MOVI_VDEB, TNHT_MOVI.MOVI_AUTO, "
            SQL += "            TNHT_FORE.FORE_DESC, "
            SQL += "            DECODE (NVL (RESE_ANPH, ''), "
            SQL += "                    '', DECODE (NVL (CCEX_TITU, ''), '', '', CCEX_TITU), "
            SQL += "                    RESE_ANPH "
            SQL += "                   ) MOVI_TITU, "
            SQL += "            TNHT_MOVI.PAPO_CODI,TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE, TNHT_MOVI.FACT_EXTE, TNHT_MOVI.SEFA_EXTE,MOVI_NUDO "
            SQL += "       FROM TNHT_MOVI, TNHT_FORE, TNHT_RESE, TNHT_CCEX "
            SQL += "      WHERE TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "        AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) "
            SQL += "        AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "        AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "        AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+) "
            SQL += "        AND TNHT_MOVI.PAPO_CODI = TNHT_CCEX.PAPO_CODI(+) "
            SQL += "   UNION ALL "
            SQL += "   SELECT   'NH' MOVI_ORIG, TNHT_MOVI.MOVI_DATR, TNHT_MOVI.RESE_CODI, "
            SQL += "            TNHT_MOVI.RESE_ANCI, TNHT_MOVI.ALOJ_CODI, TNHT_MOVI.UNMO_CODI, "
            SQL += "            TNHT_MOVI.FORE_CODI, TNHT_MOVI.MOVI_CORR, TNHT_MOVI.MOVI_ANUL, "
            SQL += "            TNHT_MOVI.MOVI_TIMO, TNHT_MOVI.FACT_CODI, TNHT_MOVI.SEFA_CODI, "
            SQL += "            TNHT_MOVI.TIRE_CODI, TNHT_MOVI.MOVI_VDEB, TNHT_MOVI.MOVI_AUTO, "
            SQL += "            TNHT_FORE.FORE_DESC, "
            SQL += "            DECODE (NVL (RESE_ANPH, ''), "
            SQL += "                    '', DECODE (NVL (CCEX_TITU, ''), '', '', CCEX_TITU), "
            SQL += "                    RESE_ANPH "
            SQL += "                   ) MOVI_TITU, "
            SQL += "            TNHT_MOVI.PAPO_CODI, TNHT_MOVI.MOVI_CODI,TNHT_MOVI.MOVI_DARE,TNHT_MOVI.FACT_EXTE, TNHT_MOVI.SEFA_EXTE,MOVI_NUDO "
            SQL += "       FROM TNHT_MOVH TNHT_MOVI, TNHT_FORE, TNHT_RESE, TNHT_CCEX "
            SQL += "      WHERE TNHT_MOVI.MOVI_TIMO = '2' "
            SQL += "        AND TNHT_MOVI.FORE_CODI = TNHT_FORE.FORE_CODI(+) "
            SQL += "        AND TNHT_MOVI.RESE_CODI = TNHT_RESE.RESE_CODI(+) "
            SQL += "        AND TNHT_MOVI.RESE_ANCI = TNHT_RESE.RESE_ANCI(+) "
            SQL += "        AND TNHT_MOVI.CCEX_CODI = TNHT_CCEX.CCEX_CODI(+) "
            SQL += "        AND TNHT_MOVI.PAPO_CODI = TNHT_CCEX.PAPO_CODI(+) "
            SQL += "   ORDER BY MOVI_DATR "

            Me.Cursor = Cursors.WaitCursor
            DB.EjecutaSql(SQL)
            If IsNothing(DB) = False Then
                If DB.EstadoConexion = ConnectionState.Open Then
                    DB.CerrarConexion()
                    DB = Nothing
                End If
            End If
            Me.Cursor = Cursors.Default


        Catch ex As Exception
            Me.Cursor = Cursors.Default
            MsgBox(ex.Message)

        End Try
    End Sub
End Class