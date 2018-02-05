SELECT 'PAGOS',movi_codi, tnpg_movi.timo_codi, movi_inte, forn_codi, forn_inte,
       movi_docu, movi_dava, movi_unmo, movi_empr, movi_llav, movi_impo,
       movi_sald, timo_form, NVL (movi_serv, '') movi_serv, movi_dept,
       timo_cdia, timo_cdoc, movi_tiim, movi_desc
  FROM tnpg_movi, tnpg_timo
 WHERE TRUNC (movi_dava) >= '27/08/2009'
   AND TRUNC (movi_dava) <= '27/08/2009'
   AND movi_impo <> 0.0
   AND tnpg_movi.timo_codi = tnpg_timo.timo_codi
   AND (   (movi_daac IS NULL)
        OR NOT (TRUNC (movi_daac) >= '27/08/2009' AND TRUNC (movi_daac) <= '27/08/2009')
       )
  AND timo_exco = 1
   AND NOT ((movi_inte = 1) AND (timo_form IN (0, 1, 2, 3)))
   
   
   
   