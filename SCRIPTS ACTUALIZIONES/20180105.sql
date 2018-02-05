

ALTER TABLE TH_PARA
 ADD (PARA_MORA_SOURCE_TYPE  VARCHAR2(25));


COMMENT ON COLUMN TH_PARA.PARA_MORA_SOURCE_TYPE IS 'Tipo de Procedencia del Movimiento';





alter session set nls_language = 'AMERICAN';
alter session set nls_territory = 'AMERICA';
alter session set nls_date_format = 'DD-MON-YYYY';
  
UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0041';
UPDATE TH_PARA SET PARA_FECHA_BASE = '05-JAN-2018';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '05-JAN-2018';

COMMIT;

-- INSTALADO 