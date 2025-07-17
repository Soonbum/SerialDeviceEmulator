# SerialDeviceEmulator

시리얼 포트 장치 에뮬레이터

* 위 프로그램을 사용하려면 윈도우 장치 관리자에서 가상으로 시리얼 포트를 생성하는 별도의 프로그램이 필요합니다.
  - 프로그램 다운로드 경로
    1) https://sourceforge.net/projects/com0com/
    2) https://github.com/vovsoft/com0com
    3) https://com0com.sourceforge.net/

1. Setup으로 들어가서 Virtual Port Pair의 이름을 변경하십시오. (접두사 COM)
2. 옵션의 의미는 다음과 같습니다.
  - use Ports class: 장치관리자의 "포트(COM & LPT)"에 들어감
  - emulate baud rate : 전송 속도 에뮬레이션 (필수)
  - enable buffer overrun : 버퍼 오버런 활성화, 속도가 너무 빠르면 신호 유실되는 것을 시뮬레이트함 (단순한 테스트라면 안 켜도 됨)
  - enable plug-in mode : 두 가상 포트 사이에 데이터가 오가는 통로에 '플러그인'이라는 별도의 프로그램을 끼워 넣을 수 있게 하는 특수 목적 기능 (안 켜도 됨)
  - enable exclusive mode : 어떤 프로그램이 연결하게 되면 다른 프로그램이 접근하지 못하게 함 (필수)
  - enable hidden mode : 사용 가능한 포트 목록에서 숨김 (안 켜도 됨)
3. 신호선 관련 옵션은 수정할 필요 없음
