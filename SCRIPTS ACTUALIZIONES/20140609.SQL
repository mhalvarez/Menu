﻿ALTER TABLE TH_PARA
 ADD (PARA_HOTEL_AX  NUMBER);



UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0040';
UPDATE TH_PARA SET PARA_FECHA_BASE = '09-JUN-2014';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '09-JUN-2014';

COMMIT;