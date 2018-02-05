


----
-- OJO CORRER COMO RMC
----
DROP VIEW VH_NIVA;

/* Formatted on 2011/07/22 14:36 (Formatter Plus v4.8.8) */
--
-- VH_NIVA  (View) 
--
CREATE OR REPLACE FORCE VIEW vh_niva (movi_codi,
                                      movi_corr,
                                      movi_dare,
                                      movi_datr,
                                      secc_codi,
                                      serv_codi,
                                      movi_vcre,
                                      movi_vliq,
                                      ncre_codi,
                                      sefa_codi,
                                      tiva,
                                      viva,
                                      movi_diar,
                                      mope_codi,
                                      papo_codi,
                                      ncre_daem,
                                      enti_codi,
                                      ccex_codi
                                     )
AS
   SELECT movi_codi, movi_corr, movi_dare, movi_datr, secc_codi, serv_codi,
          movi_vcre, movi_vliq, m.ncre_codi, m.sefa_codi, movi_iva1,
          movi_imp1, movi_diar, mope_codi, m.papo_codi, ncre_daem,
          n.enti_codi, n.ccex_codi
     FROM tnht_movi m, tnht_ncre n
    WHERE m.movi_timo = '1'
      AND m.movi_iva1 IS NOT NULL
      AND m.movi_imp1 <> 0
      AND m.ncre_codi IS NOT NULL
      AND m.ncre_codi = n.ncre_codi
      AND m.sefa_codi = n.sefa_codi
   UNION ALL
   SELECT movi_codi, movi_corr, movi_dare, movi_datr, secc_codi, serv_codi,
          movi_vcre, movi_vliq, m.ncre_codi, m.sefa_codi, movi_iva2,
          movi_imp2, movi_diar, mope_codi, m.papo_codi, ncre_daem,
          n.enti_codi, n.ccex_codi
     FROM tnht_movi m, tnht_ncre n
    WHERE m.movi_timo = '1'
      AND m.movi_iva2 IS NOT NULL
      AND m.movi_imp2 <> 0
      AND m.ncre_codi IS NOT NULL
      AND m.ncre_codi = n.ncre_codi
      AND m.sefa_codi = n.sefa_codi
   UNION ALL
   SELECT movi_codi, movi_corr, movi_dare, movi_datr, secc_codi, serv_codi,
          movi_vcre, movi_vliq, m.ncre_codi, m.sefa_codi, movi_iva3,
          movi_imp3, movi_diar, mope_codi, m.papo_codi, ncre_daem,
          n.enti_codi, n.ccex_codi
     FROM tnht_movi m, tnht_ncre n
    WHERE m.movi_timo = '1'
      AND m.movi_iva3 IS NOT NULL
      AND m.movi_imp3 <> 0
      AND m.ncre_codi IS NOT NULL
      AND m.ncre_codi = n.ncre_codi
      AND m.sefa_codi = n.sefa_codi
   UNION ALL
   SELECT movi_codi, movi_corr, movi_dare, movi_datr, secc_codi, serv_codi,
          movi_vcre, movi_vliq, m.ncre_codi, m.sefa_codi, movi_iva3,
          movi_imp3, movi_diar, mope_codi, m.papo_codi, ncre_daem,
          n.enti_codi, n.ccex_codi
     FROM tnht_movi m, tnht_ncre n
    WHERE m.movi_timo = '1'
      AND m.movi_imp1 = 0
      AND m.movi_imp2 = 0
      AND m.movi_imp3 = 0
      AND m.ncre_codi IS NOT NULL
      AND m.ncre_codi = n.ncre_codi
      AND m.sefa_codi = n.sefa_codi;

UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0025';
UPDATE TH_PARA SET PARA_FECHA_BASE = '22-JUL-2011';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '22-JUL-2011';

COMMIT;