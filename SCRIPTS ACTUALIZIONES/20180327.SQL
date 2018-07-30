

ALTER TABLE TC_ASNT
 ADD (ASNT_BANCOS_NOT  VARCHAR2(10));


 COMMENT ON COLUMN TC_ASNT.ASNT_BANCOS_NOT IS 'Banco de la Notificacion Spyro';


alter session set nls_language = 'AMERICAN';
alter session set nls_territory = 'AMERICA';
alter session set nls_date_format = 'DD-MON-YYYY';
  
UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0041';
UPDATE TH_PARA SET PARA_FECHA_BASE = '27-MAR-2018';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '27-MAR-2018';

COMMIT;
-- instalado 