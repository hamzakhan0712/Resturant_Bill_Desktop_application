   M S B A M L   `   `   `  ����U  QRestaurantManagementSystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:RestaurantManagementSystemRestaurantManagementSystem  5      1    +RestaurantManagementSystem.EditMenuItemCard   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      :local(clr-namespace:RestaurantManagementSystem     5      .��5      �� -   5          ��Name$  MenuItemCard��5      
��� d �$��1q�6;   $��0 0 0 10q�6O    �� CornerRadius$ 4��6a   ._�5   
   �  � RowDefinitions 5	      �� 5
      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    �� 5      $
��Auto �6    .+�5      �� -   5         ��Name$ 
CloseClick��5      $��X��6B   $
��Left=�6N   $	��Top=�6i   $��0q�6�   
������ 
���  ��$	��5 0q�6�   $��15(�6�    � Cursor$
 Hand��6�   �� 5      $��1��6    �� Orientation$ 
Horizontal=�6&   $��40,5 5 5q�6?   $��Center=�6Q   $
��Left=�6l   .+�5      �� $��Name:��6   $��0 0 5 3q�6)   $	��Top=�6:   $��12(�6R   �� 5      -   5         ��Name$ TxtBoxNewItemName��5      $��1q�65   $��14(�6I   $	��200��6W   $��5 0 5 5q�6c   $��5q�6t   5      �� 5      -   5         ��Name$ CategoryComboBox��5      $��2��61   $	��200��6>   $��35��6J   $��40 5 5 5q�6V   .@�5      �� $	��Veg��6   �� 5      $��Non-Veg��6   �� 5       $��Drinks��6   �� 5!      $��Dessert��6   5#      �� 5&      $��3��6   $ 
Horizontal=�6&   $��40 5 5 5q�6?   .+�5'      �� $��Note:��6   $��0 0 5 0q�6)   $	��Top=�6:   $��12(�6R   �� 5(      -   5        $ 
TxtBoxNote��5(      $��1q�6.   $��14(�6B   $	��200��6P   $��5 0 5 5q�6\   $��3q�6m   5)      �� 5,      $��4��6   $ 
Horizontal=�6&   $��40 5 5 5q�6?   .+�5-      �� $��AC Price (Half):��6   $��0 0 5 3q�64   $	��Top=�6E   $��12(�6]   �� 5.      -   5        $ TxtBoxNewItemACPriceHalf��5.      $��1q�6<   $��14(�6P   $	��200��6^   $��5 0 5 5q�6j   $��5q�6{   5/      �� 52      $��5��6   $ 
Horizontal=�6&   $��40 5 5 5q�6?   .+�53      �� $��AC Price (Full):��6   $��0 0 5 3q�64   $	��Top=�6E   $��12(�6]   �� 54      -   5        $ TxtBoxNewItemACPriceFull��54      $��1q�6<   $��14(�6P   $	��200��6^   $��5 0 5 5q�6j   $��5q�6{   55      �� 58      $��6��6   $ 
Horizontal=�6&   $��40 5 5 5q�6?   .+�59      �� $��Non-AC Price (Half):��6   $��0 0 5 3q�68   $	��Top=�6I   $��12(�6a   �� 5:      -   5        $! TxtBoxNewItemNonACPriceHalf��5:      $��1q�6?   $��14(�6S   $	��200��6a   $��5 0 5 5q�6m   $��5q�6~   5;      �� 5>      $��7��6   $ 
Horizontal=�6&   $��40 5 5 5q�6?   .+�5?      �� $��Non-AC Price (Full):��6   $��0 0 5 3q�68   $	��Top=�6I   $��12(�6a   �� 5@      -	   5        $! TxtBoxNewItemNonACPriceFull��5@      $��1q�6?   $��14(�6S   $	��200��6a   $��5 0 5 5q�6m   $��5q�6~   5A      �� 5D      -
   5        $ 	SaveClick��5D      $
��Save��6A   $��	10 0 10 0q�6P   ���#4285F4$��Right=�6y   $��15(�6�   $��Normal"�6�   $��Bottom=�6�   $��10q�6�   $��8��6�   5E      5F      5G      