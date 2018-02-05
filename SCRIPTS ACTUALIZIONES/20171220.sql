﻿


ALTER TABLE TH_PARA
 ADD (PARA_LOPEZ_TIPO_COMPROBANTES  NUMBER          DEFAULT 0);


COMMENT ON COLUMN TH_PARA.PARA_LOPEZ_TIPO_COMPROBANTES IS 'Tipo de gestion de Comprobantes 0=Antigua 1=Nueva';





CREATE TABLE TC_COMP
(
  TACO_CODI  VARCHAR2(15),
  MOCO_CODI  NUMBER,
  COMP_NUME  NUMBER,
  TIMO_CECO  VARCHAR2(15)
)
LOGGING 
NOCOMPRESS 
NOCACHE
NOPARALLEL
NOMONITORING;

COMMENT ON TABLE TC_COMP IS 'Comprobantes asociados a Pagos recibidos';


ALTER TABLE TC_COMP ADD (
  CONSTRAINT TC_COMP_PK
  PRIMARY KEY
  (TACO_CODI, MOCO_CODI));



alter session set nls_language = 'AMERICAN';
alter session set nls_territory = 'AMERICA';
alter session set nls_date_format = 'DD-MON-YYYY';
  
UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0041';
UPDATE TH_PARA SET PARA_FECHA_BASE = '20-DEC-2017';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '20-DEC-2017';

COMMIT;
-- INSTALADO
