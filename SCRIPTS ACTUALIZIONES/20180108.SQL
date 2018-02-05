ALTER TABLE TS_PARA
 ADD (PARA_SERIE_ANIO_2B  NUMBER                    DEFAULT 0);


COMMENT ON COLUMN TS_PARA.PARA_SERIE_ANIO_2B IS 'Si el Año de la serie es de 2 bytes = 1';


ALTER TABLE TS_ASNT
 ADD (ASNT_FACTURA_SERIE  VARCHAR2(25 BYTE));





alter session set nls_language = 'AMERICAN';
alter session set nls_territory = 'AMERICA';
alter session set nls_date_format = 'DD-MON-YYYY';
  
UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0041';
UPDATE TH_PARA SET PARA_FECHA_BASE = '08-JAN-2018';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '08-JAN-2018';

COMMIT;
-- ya instalado 