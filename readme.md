# Beehive Chess.

## Rules

Rules are simple!
- Place 5 pieces with same color in a row( horizontal or diagonal).
- Place 6 pieces with same color form a ring.

How to play:
- Tap to the board to place your piece, then tap "Go" to end your turn.

Cách chơi tương tự cờ caro,
Mỗi lượt chơi người chơi đặt 1 quân cờ xuống bàn.
Xếp 5 mảnh cùng màu liên tiếp thành hàng(dọc hoặc chéo) hoặc 6 mảnh thành 1 vòng tròn cùng màu để chiến thắng.

## Sample Game.

Đây là bản game đầy đủ theo luật chơi bên trên.
https://play.google.com/store/apps/details?id=com.setik.behivechess

## SampleProject

Đây là bản game thiếu logic để AI biết cách xếp quân tạo thành 1 vòng tròn đồng màu để chiến thắng
This sample project missing the logic to allow the AI to win game by completing a ring with 6 pieces of the same color

## Missions.

### Mission 1

Hoàn thiện logic còn thiếu để AI biết các xếp quân tạo thành 1 vòng tròn để chiến thắng.
Complete the logic which allow AI to win the game by completing a ring with 6 pieces of the same color.

Function cần hoàn thiện:
GetRingConsecutive(int i, int j, sbyte refCell)
EvaluateRing(bool forYellow, bool yellowTurn)

Hướng dẫn cụ thể được chú thích trong các hàm này.

### Mission 2(optional-có thể làm hoặc không)

Thuật toán đánh giá điểm EvaluateScore() của 1 bàn chơi đang chạy trên tất cả các vị trí có thể đặt được quân cờ, tìm cách để loại bớt các vị trí không đem lại giá trị cao để giúp thuật toán chạy nhanh hơn.

## Hints

Có thể thêm 1 text field trong prefab Assets/Prefabs/Platform.prefab, Assets/Prefabs/Cell.prefab để biết vị trí cell tương ứng với tọa độ x,z là bao nhiêu.
Cần cài Unity 2019.4.40f1 để project ko bị lỗi khi upgrade lên phiên bản khác.

