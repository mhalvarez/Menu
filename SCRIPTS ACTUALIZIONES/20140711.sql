﻿ALTER TABLE TC_ASNT
 ADD (ASNT_DESC_DEBITO  VARCHAR2(256));

ALTER TABLE TC_ASNT
 ADD (ASNT_DESC_CREDITO  VARCHAR2(256));

ALTER TABLE TC_ASNT
 ADD (ASNT_DOC_CREDITO  VARCHAR2(256));

ALTER TABLE TC_ASNT
 ADD (ASNT_DOC_DEBITO  VARCHAR2(256));


COMMENT ON COLUMN TC_ASNT.ASNT_DESC_CREDITO IS 'Descripcion del Movimiento de Credito';

COMMENT ON COLUMN TC_ASNT.ASNT_DESC_DEBITO IS 'Descripcion del Movimiento de Debito';

COMMENT ON COLUMN TC_ASNT.ASNT_DOC_CREDITO IS 'Documento de Credito';

COMMENT ON COLUMN TC_ASNT.ASNT_DOC_DEBITO IS 'Documento de Debito';


UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0040';
UPDATE TH_PARA SET PARA_FECHA_BASE = '11-JUL-2014';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '11-JUL-2014';

COMMIT;