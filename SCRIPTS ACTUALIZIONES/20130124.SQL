﻿


ALTER TABLE TS_ASNT
 ADD (ASNT_TIPO_IGIC  NUMBER);


ALTER TABLE TS_ASNT
 ADD (ASNT_AUX_UPDATE  VARCHAR2(512));


COMMENT ON COLUMN TS_ASNT.ASNT_AUX_UPDATE IS 'Auxiliar Update cambio de estado';


UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0040';
UPDATE TH_PARA SET PARA_FECHA_BASE = '24-JAN-2013';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '24-JAN-2013';

COMMIT;