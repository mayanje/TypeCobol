﻿       IDENTIFICATION DIVISION.
       PROGRAM-ID. PERFORM0.
       ENVIRONMENT DIVISION.
       DATA DIVISION.
       WORKING-STORAGE SECTION.
       01 a PIC 9(02).
       01 b PIC 9(02).
       PROCEDURE DIVISION.
           PERFORM
             MOVE a TO b;
             DISPLAY a;
           END-PERFORM.
       END PROGRAM PERFORM0.
