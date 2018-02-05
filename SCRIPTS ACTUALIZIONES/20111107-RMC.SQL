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
                                      ncre_docu,
                                      sedo_codi
                                     )
AS
   SELECT /*+ INDEX(INHT_MCRE_01)*/
          movi.movi_codi, movi.movi_corr, movi.movi_dare, movi.movi_datr,
          movi.secc_codi, movi.serv_codi, movi.movi_vcre, movi.movi_vliq,
          mcre.ncre_codi, mcre.sefa_codi, movi.movi_iva1, movi.movi_imp1,
          movi.movi_diar, movi.mope_codi, movi.papo_codi, ncre_daem,
          enti_codi, ncre_docu, sedo_codi
     FROM tnht_mcre mcre, vnht_movh movi, tnht_ncre n
    WHERE mcre.movi_codi = movi.movi_codi
      AND mcre.movi_dare = movi.movi_dare
      AND movi.movi_timo = '1'
      AND movi.movi_iva1 IS NOT NULL
      AND mcre.ncre_codi = n.ncre_codi
      AND mcre.sefa_codi = n.sefa_codi
   UNION ALL
   SELECT /*+ INDEX(INHT_MCRE_01)*/
          movi.movi_codi, movi.movi_corr, movi.movi_dare, movi.movi_datr,
          movi.secc_codi, movi.serv_codi, movi.movi_vcre, movi.movi_vliq,
          mcre.ncre_codi, mcre.sefa_codi, movi.movi_iva2, movi.movi_imp2,
          movi.movi_diar, movi.mope_codi, movi.papo_codi, ncre_daem,
          enti_codi, ncre_docu, sedo_codi
     FROM tnht_mcre mcre, vnht_movh movi, tnht_ncre n
    WHERE mcre.movi_codi = movi.movi_codi
      AND mcre.movi_dare = movi.movi_dare
      AND movi.movi_timo = '1'
      AND movi.movi_iva2 IS NOT NULL
      AND mcre.ncre_codi = n.ncre_codi
      AND mcre.sefa_codi = n.sefa_codi
   UNION ALL
   SELECT /*+ INDEX(INHT_MCRE_01)*/
          movi.movi_codi, movi.movi_corr, movi.movi_dare, movi.movi_datr,
          movi.secc_codi, movi.serv_codi, movi.movi_vcre, movi.movi_vliq,
          mcre.ncre_codi, mcre.sefa_codi, movi.movi_iva3, movi.movi_imp3,
          movi.movi_diar, movi.mope_codi, movi.papo_codi, ncre_daem,
          enti_codi, ncre_docu, sedo_codi
     FROM tnht_mcre mcre, vnht_movh movi, tnht_ncre n
    WHERE mcre.movi_codi = movi.movi_codi
      AND mcre.movi_dare = movi.movi_dare
      AND movi.movi_timo = '1'
      AND movi.movi_iva3 IS NOT NULL
      AND mcre.ncre_codi = n.ncre_codi
      AND mcre.sefa_codi = n.sefa_codi
   UNION ALL
   SELECT /*+ INDEX(INHT_MCRE_01)*/
          movi.movi_codi, movi.movi_corr, movi.movi_dare, movi.movi_datr,
          movi.secc_codi, movi.serv_codi, movi.movi_vcre, movi.movi_vliq,
          mcre.ncre_codi, mcre.sefa_codi, movi.movi_iva3, movi.movi_imp3,
          movi.movi_diar, movi.mope_codi, movi.papo_codi, ncre_daem,
          enti_codi, ncre_docu, sedo_codi
     FROM tnht_mcre mcre, vnht_movh movi, tnht_ncre n
    WHERE mcre.movi_codi = movi.movi_codi
      AND mcre.movi_dare = movi.movi_dare
      AND movi.movi_timo = '1'
      AND movi_imp1 = 0
      AND movi_imp2 = 0
      AND movi_imp3 = 0
      AND movi.movi_iva1 IS NULL
      AND movi.movi_iva2 IS NULL
      AND movi.movi_iva3 IS NULL
      AND mcre.ncre_codi = n.ncre_codi
      AND mcre.sefa_codi = n.sefa_codi
/


-- ojo como usuario integracion
UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0030';
UPDATE TH_PARA SET PARA_FECHA_BASE = '07-NOV-2011';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '07-NOV-2011';

COMMIT;

