﻿ALTER TABLE INTEGRACION.TH_PARA
 ADD (PARA_CFATODIARI_COD_2  VARCHAR2(4));


COMMENT ON COLUMN INTEGRACION.TH_PARA.PARA_CFATODIARI_COD_2 IS 'Diario de Spyro para Apuntes de Efectivo / Visa ( cobros ) en Asiento de Ventas';







ALTER TABLE INTEGRACION.TH_PARA
 ADD (PARA_TRATA_CAJA  NUMBER                       DEFAULT 0);


COMMENT ON COLUMN INTEGRACION.TH_PARA.PARA_TRATA_CAJA IS 'Si se contabilizan Anticipos y Devoluciones H.Lopez';


---  hasta arriba ya esta ejecutado en h lopez 


ALTER TABLE INTEGRACION.TS_ASNT
 ADD (ASNT_ALMA_DESC  VARCHAR2(250));


COMMENT ON COLUMN INTEGRACION.TS_ASNT.ASNT_ALMA_DESC IS 'Descripción del Almacen';




UPDATE TH_PARA SET PARA_FECHA_BASE = '18-JUN-2010';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '18-JUN-2010';

COMMIT;