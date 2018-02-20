ALTER TABLE TH_PARA
MODIFY(PARA_MORA_SOURCE_TYPE VARCHAR2(25 BYTE));


ALTER TABLE TH_PARA
 ADD (PARA_TEFECT_COD  VARCHAR2(4 BYTE));


COMMENT ON COLUMN TH_PARA.PARA_TEFECT_COD IS 'Tipo de efecto spyro';


ALTER TABLE TC_PARA
 ADD (PARA_BANCOS_COD  VARCHAR2(8 BYTE));

ALTER TABLE TC_PARA
 ADD (PARA_BANCOS_COD2  VARCHAR2(8));


COMMENT ON COLUMN TC_PARA.PARA_BANCOS_COD IS 'Banco para Cobros con Visa';

COMMENT ON COLUMN TC_PARA.PARA_BANCOS_COD2 IS 'Banco para Cobros con Transferencias Bancarias';


ALTER TABLE TG_TRANS
 ADD (TRANS_FCRE  TIMESTAMP(6));


 CREATE OR REPLACE TRIGGER TG_TRANS_CLIE_H1
AFTER DELETE OR INSERT OR UPDATE
ON TNHT_ENTI REFERENCING NEW AS NEW OLD AS OLD
FOR EACH ROW
DECLARE
   vcodigo   NUMBER;
   vcontrol  NUMBER;
BEGIN
   SELECT TG_TRANS_SEC.NEXTVAL INTO vcodigo FROM DUAL;
   SELECT DECODE (COUNT(*) ,0,0,1)  INTO VCONTROL FROM TG_TRANS WHERE TRANS_PKEY = :NEW.ENTI_CODI AND TRANS_CEXT <> :NEW.ENTI_CAUX  AND TRANS_STAT = 1;
 

   IF INSERTING
   THEN
      INSERT INTO TG_TRANS (TRANS_CODI,TRANS_EMPGRUPO_COD,TRANS_EMP_COD,TRANS_EMP_NUM,
                            TRANS_TABL,
                             TRANS_PKEY,
                             TRANS_CEXT,
                            TRANS_TYPE,
                            TRANS_STAT,
                            TRANS_HOTE,
                            TRANS_ERRO,TRANS_LOCK,TRANS_FCRE)
           VALUES (vcodigo,'GRTI','1',1,
                   'TNHT_ENTI',
                   :NEW.ENTI_CODI,
                   :NEW.ENTI_CAUX,
                   1,
                   0,
                   0,
                   NULL,vcontrol,SYS_EXTRACT_UTC(SYSTIMESTAMP));
   END IF;


   IF UPDATING
   THEN
      INSERT INTO TG_TRANS (TRANS_CODI,TRANS_EMPGRUPO_COD,TRANS_EMP_COD,TRANS_EMP_NUM,
                            TRANS_TABL,
                            TRANS_PKEY,
                              TRANS_CEXT,
                            TRANS_TYPE,
                            TRANS_STAT,
                            TRANS_HOTE,
                            TRANS_ERRO,TRANS_LOCK,TRANS_FCRE)
           VALUES (vcodigo,'GRTI','1',1,
                   'TNHT_ENTI',
                    :OLD.ENTI_CODI,
                    :NEW.ENTI_CAUX,
                   2,
                   0,
                   0,
                   NULL,vcontrol,SYS_EXTRACT_UTC(SYSTIMESTAMP));
   END IF;



   IF DELETING
   THEN
      INSERT INTO TG_TRANS (TRANS_CODI,TRANS_EMPGRUPO_COD,TRANS_EMP_COD,TRANS_EMP_NUM,
                            TRANS_TABL,
                             TRANS_PKEY,
                              TRANS_CEXT,
                            TRANS_TYPE,
                            TRANS_STAT,
                            TRANS_HOTE,
                            TRANS_ERRO,TRANS_LOCK,TRANS_FCRE)
           VALUES (vcodigo,'GRTI','1',1,
                   'TNHT_ENTI',
                     :OLD.ENTI_CODI,
                      :OLD.ENTI_CAUX,
                   3,
                   0,
                   0,
                   NULL,vcontrol,SYS_EXTRACT_UTC(SYSTIMESTAMP));
   END IF;
EXCEPTION
   WHEN OTHERS
   THEN
      -- Consider logging the error and then re-raise
      
     RAISE;
  --    raise_application_error(-20001,'ERROR');
END TG_TRANS_CLIE_H1;
/


ALTER TABLE TG_TRANS
 ADD (TRANS_NAV_KEY  VARCHAR2(100));


COMMENT ON COLUMN TG_TRANS.TRANS_NAV_KEY IS 'Suerte de Primary key en navision';

alter session set nls_language = 'AMERICAN';
alter session set nls_territory = 'AMERICA';
alter session set nls_date_format = 'DD-MON-YYYY';
  
UPDATE TH_PARA SET PARA_PAT_NUM = 'PAT0041';
UPDATE TH_PARA SET PARA_FECHA_BASE = '30-JAN-2018';
UPDATE TH_PARA SET PARA_FECHA_EXE  = '30-JAN-2018';

COMMIT;