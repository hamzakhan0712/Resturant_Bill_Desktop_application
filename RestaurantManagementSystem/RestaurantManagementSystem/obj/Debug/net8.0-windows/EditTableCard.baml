   M S B A M L   `   `   `  ����U  QRestaurantManagementSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:RestaurantManagementSystemRestaurantManagementSystem  5      .    (RestaurantManagementSystem.EditTableCard   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      :local(clr-namespace:RestaurantManagementSystem     5      .��5	      � .+�5
   
   �� -   5          ��Name$  
TablesCard��5
      
��� d �$��1q�6=   $��0 0 0 10q�6Q    �� CornerRadius$ 4��6c   ._�5      �  � RowDefinitions 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� -   5         ��Name$ 
CloseClick��5      $��X��6F   $
��Left=�6R   $	��Top=�6m   $��0q�6�   
������ 
���  ��$	��5 0q�6�   $��15(�6�    � Cursor$
 Hand��6�   �� 5      $��1��6    �� Orientation$ 
Horizontal=�6*   $��40,5 5 5q�6C   $��Center=�6U   $
��Left=�6p   .+�5      �� $
��15,5q�6!   .+�5      �� $��	Table ID:��6$   $��0 0 5 3q�65   $	��Top=�6F   $��12(�6^   �� 5      $ 4��6!   ���#4285F4$��1q�6H   ._�5      �� -   5         ��Name$ TxtBoxEditTableId��5   &    �� 
IsReadOnly$
 True��6A   $��0q�6S   $��14(�6g   $	��200��6u   $��5 0 5 5q�6�   $��5q�6�   5      5      �� 5!      $
��15,5q�6!   .+�5"      �� $��Seating Capacity:��6$   $��0 0 5 3q�6=   $	��Top=�6N   $��12(�6f   �� 5#      $ 4��6!   ���#4285F4$��1q�6H   ._�5$      �� -   5        $$ TxtBoxEditTableSeatingCapacity��5$   &   $��0q�6N   $��14(�6b   $	��200��6p   $��5 0 5 5q�6|   $��5q�6�   5%      5&      �� 5(      -   5         ��Name$ TableTypeComboBox��5(      $	��200��6:   $��35��6F   $��
15 20 30 5q�6R   .@�5)      �� $��AC��6'   �� 5*      $��Non-AC��6'   5,      �� 5.      -   5        $ StatusComboBox��5.      $	��200��67   $��35��6C   $��
15 20 30 5q�6O   .@�5/      �� $��	Available��6'   �� 50      $��Occupied��6'   �� 51      $��Reserved��6'   53      56      �� 59      -   5        $ 	SaveClick��59      $
��Save��6E   $��	10 0 10 0q�6T   ���#4285F4$��Right=�6}   $��15(�6�   $��Normal"�6�   $��Bottom=�6�   $��10q�6�   $��4��6�   5:      5;      5<      5=      