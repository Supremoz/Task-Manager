   M S B A M L   `   `   `  ����G  CTask Manager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Task_ManagerTask_Manager  5   	        Task_Manager.TaskDashboard   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Task_Manager     5   	   -   5          � Title$  Task Manager��5   	   $	��770��6   $
��1400��6+   $	��385��68   $	��700��6H    � WindowStartupLocation$ CenterScreen=�6W    � Icon$ /Resources/logo.png��6|   ���#F8FCFF � WindowState$ 	Maximized=�5	       � 	Resources 5
         CustomButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType	 �� Triggers
 P� Property P� Value  MouseOverButtonStyle �� CornerRadius �� Triggers3  -System.Windows.Media.Effects.DropShadowEffect   
BlurRadius   ShadowDepth   Color  AddMouseOverButtonStyle ��Name �� 
TargetName  CreateButtonStyle/   )Task_Manager.BooleanToFontWeightConverter   BooleanToFontWeightConverter��%�
  &	        &	     &	 V    &	 L    &	 �
    �� 5   
   " ��6*   .�5      ��  � ��
 ���� 6+   �� 5       � ��
 ���� 6,   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   #��z��6K   $��0q�6w   ._�5      �� $��Center=�6)   $��Center=�6F   .+�5   "   �� $��Center=�63   $��Center=�6P   5      5      	 5      P� 5      
 �@J�IsMouseOver$
 True��6=   . �5   "   ��  � ��
 ���� 6?   �� 5   "    � ��
 ���� 6@   5      5      5      5      �� 5    
   " ��6-   .�5!      ��  � �� �#DAEAFA6+   �� 5"       � ��
 �   �6+   �� 5#       � ��$ /Fonts/#Pontano Sans)�6+   �� 5$       � ��
 ���� 6,   �� 5%       � ��$ 0q�6(   �� 5&       � �� 5'      �� 5(      " ��6&   .R�5)      �� #��z��5*      #��z��5+      #��z��5,      $ 4��5-      ._�5/      �� $
��Left=�50      $��Center=�51      #��z��52      #��z��53      54      55      57       58      P� 59      
 �@J�IsMouseOver$
 True��61   . �5:      ��  � �� �#80BFFF63   �� 5;       � ��
 �   �64   �� 5<       � �� �#00000063   �� 5=       � ��$
 Bold"�63   �� 5>       �@J�Effect 5?        5@      $ 10Q�6/   $ 2Q�6?   $ #888888��6O   5B      5C      5E      �� 5F   
   " ��60   .�5H      ��  � ��
 ���� 6+   �� 5I       � ��
 ���� 6,   �� 5J       � ��
 �   �6+   �� 5K       � ��$	 1.5q�6(   �� 5L       � �� 5M      �� 5N      " ��6&   .R�5P      �� $ ButtonBorder��6!   #��z��67   #��z��6a   $��0q�6�   ._�5Q      �� $��Center=�6/   $��Center=�6L   5R      	 5S      P� 5U      
 �@J�IsMouseOver$
 True��6=   . �5V   "   �� $ ButtonBorder��6)    � ��
 ���� 6Y   �� 5W   "   $ ButtonBorder��6)    � ��
 ���� 6Z   5X      5Z      5\       5]      P� 5_      
 �@J�IsMouseOver$
 True��61   . �5`      ��  � �� �#00000063   �� 5a       � ��$
 Bold"�63   5b      5d      �� 5e   
   " ��6*   .�5f      ��  � ��$ 16(�6)   �� 5g       � ��$ /Fonts/#Pontano Sans Bold)�6+   �� 5h       � �� �#BFD1E36+   �� 5i       � ��
 �   �6+   �� 5j       � ��$ Right=�64   �� 5k       � ��$ Center=�62   �� 5l       � ��$ 10q�6(   �� 5m       � �� �#BFD1E36,   �� 5n       � ��$ 1q�60   �� 5o       � �� 5p      �� 5q      " ��6&   .R�5r      �� #��z��6!   #��z��5s      #��z��5t      $ 10��5u      ._�5v      �� $��Center=�6/   $��Center=�6L   5w      5x      5z      5{        5|   
   6-   5}      .��5~      � $��30,30,30,30q�6    � ColumnDefinitions 5   
   �� 5�      $	��230 �6   �� 5�      $��* �6   .+�5�   
   �� ���#ECF7FF$��0q�6&   $��1q�61   $ 12��6E   $	��690��6W   ._�5�      �� ���#ECF7FF$��0q�6*   $ 12��65   $	��680��6G   ��5�      �� 5�       �� EndPoint$ 0.5,1&�6*    �� 
StartPoint$ 0.5,0&�6;   .��5�      �  � Color$ Black��6'   � 5�      $ Black��6'    � Offset$ 1Q�65   5�      ._�5�      �� $��0q�6   $	��Top=�6(   .+�5�      �� $	��100��6   $	��100��6)   $��1q�66   $ 50��6J   $��0,15,0,5q�6\   
���   ���5�      �� 5�       �� ImageSource$ /Resources/side_user.png��6)    j� Stretch$ UniformToFill=�6P   5�      �� 5�       �� Orientation$ 
Horizontal=�6!   $��Center=�6:   $��5q�6W   .+�5�      �� $��/Fonts/#Roboto Slab)�6$   $��Username��6E   $��18(�6U   $
��Bold"�6c   ���#444$��0,0,5,0q�6�   5�      �� 5�      $��1q�6   $ 13��61   $��
20,15,25,0q�6C    �� Padding$ 5q�6W   $
��Left=�6c   ._�5�      �� -   5         ��Name$ DashboardBtn��5�   !   #��[ 67   $��38��6e   $	��170��6q    � Cursor$
 Hand��6}   $
��Left=�6�   .��5�      �� $ 
Horizontal=�6)   $
��Left=�6B   $��10,0,0,0q�6]   .+�5�   "   �� $#��/Resources/side_dashboard.png��6(   $��20��6O   $��20��6Z   $��0,0,10,0q�6f   z�5�   &   �� 5�   *   -   5        DashboardImageScale�� �� ScaleX$ 1Q�5�   V    �� ScaleY$ 1Q�6a     J� RenderTransformOrigin$  0.5,0.5&�5�   g   5�   #   �� 5�   "   $��	Dashboard��6,   $��17(�6=   $��Center=�6K   5�      ! � Triggers! 5�      :� 5�   "   " :� RoutedEvent$" 
MouseEnter��6/   .[�5�   &   �� .��5�   *   �� .��5�   .   W� # �� 
TargetName$# DashboardImageScale��6>   $ �� TargetProperty$$ ScaleX�6j   	% W� To% 1.156�   & g� Duration$& 0:0:0.3D�6�   W� 5�   .   $# DashboardImageScale��6>   $$ ScaleY�6j   % 1.156�   $& 0:0:0.3D�6�   5�   +   5�   '   5�   #   :� 5�   "   $" 
MouseLeave��6/   .[�5�   &   �� .��5�   *   �� .��5�   .   W� $# DashboardImageScale��6>   $$ ScaleX�6j   % 16�   $& 0:0:0.3D�6�   W� 5�   .   $# DashboardImageScale��6>   $$ ScaleY�6j   % 16�   $& 0:0:0.3D�6�   5�   +   5�   '   5�   #   5�      5�      �� 5�      $��1q�6   $ 13��61   $��	20,0,25,0q�6C   $ 5q�6V   ._�5�      �� -   5        $ 
SummaryBtn��5�   !   #��[ 65   $��38��6c   $
 Hand��6o   $
��Left=�6}   $��Center=�6�   .��5�      �� $ 
Horizontal=�6)   $
��Left=�6B   $��10,0,0,0q�6]   .+�5�   "   �� $!��/Resources/side_summary.png��6(   $��20��6M   $��20��6X   $��0,0,10,0q�6d   z�5�   &   �� 5�   *   -   5        SummaryImageScale��$ 1Q�5�   T   $ 1Q�6_   $  0.5,0.5&�5�   g   5�   #   �� 5�   "   $��Task Summary��6,   $��17(�6@   $��Center=�6N   5�      ! 5�      :� 5�   "   $" 
MouseEnter��6/   .[�5�   &   �� .��5�   *   �� .��5�   .   W� $# SummaryImageScale��6>   $$ ScaleX�6h   % 1.156�   $& 0:0:0.3D�6�   W� 5�   .   $# SummaryImageScale��6>   $$ ScaleY�6h   % 1.156�   $& 0:0:0.3D�6�   5�   +   5�   '   5�   #   :� 5�   "   $" 
MouseLeave��6/   .[�5�   &   �� .��5�   *   �� .��5�   .   W� $# SummaryImageScale��6>   $$ ScaleX�6h   % 16�   $& 0:0:0.3D�6�   W� 5�   .   $# SummaryImageScale��6>   $$ ScaleY�6h   % 16�   $& 0:0:0.3D�6�   5�   +   5�   '   5�   #   5�      5�      �� 5�      $��1q�6   $ 13��61   $��	20,0,25,0q�6C   $ 5q�6V   ._�5�      �� -   5        $ AdminBtn��5�   !   #��[ 63   $��38��6a   $
 Hand��6m   $
��Left=�6{   .��5�      �� $ 
Horizontal=�6)   $��Center=�6B   $��10,0,0,0q�6]   .+�5�   "   �� $��/Resources/side_admin.png��6(   $��20��6K   $��20��6V   $��0,2,10,0q�6b   z�5�   &   �� 5�   *   -   5        AdminImageScale��$ 1Q�5�   R   $ 1Q�6]   $  0.5,0.5&�5�   g   5�   #   �� 5�   "   $��Admin��6,   $��17(�69   $��Center=�6G   5�      ! 5�      :� 5�   "   $" 
MouseEnter��6/   .[�5�   &   �� .��5�   *   �� .��5�   .   W� $# AdminImageScale��6>   $$ ScaleX�6f   % 1.156�   $& 0:0:0.3D�6�   W� 5�   .   $# AdminImageScale��6>   $$ ScaleY�6f   % 1.156�   $& 0:0:0.3D�6�   5�   +   5�   '   5�   #   :� 5�   "   $" 
MouseLeave��6/   .[�5�   &   �� .��5�   *   �� .��5�   .   W� $# AdminImageScale��6>   $$ ScaleX�6f   % 16�   $& 0:0:0.3D�6�   W� 5�   .   $# AdminImageScale��6>   $$ ScaleY�6f   % 16�   $& 0:0:0.3D�6�   5�   +   5�   '   5�   #   5�      5�      �� 5�      $��1q�6   $ 13��61   $��	20,0,25,0q�6C   $ 5q�6V   ._�5      �� -   5        $ 	LogoutBtn��5   !   #��[ 64   $��38��6b   $
 Hand��6n   $
��Left=�6|   .��5     �� $ 
Horizontal=�6)   $��Center=�6B   $��10,0,0,0q�6]   .+�5  "   �� $ ��/Resources/side_logout.png��6(   $��20��6L   $��20��6W   $��0,2,10,0q�6c   z�5  &   �� 5  *   -	   5        LogoutImageScale��$ 1Q�5  S   $ 1Q�6^   $  0.5,0.5&�5  g   5  #   �� 5  "   $��Logout��6,   $��17(�6:   $��Center=�6H   5	     ! 5
     :� 5  "   $" 
MouseEnter��6/   .[�5  &   �� .��5  *   �� .��5  .   W� $# LogoutImageScale��6>   $$ ScaleX�6g   % 1.156�   $& 0:0:0.3D�6�   W� 5  .   $# LogoutImageScale��6>   $$ ScaleY�6g   % 1.156�   $& 0:0:0.3D�6�   5  +   5  '   5  #   :� 5  "   $" 
MouseLeave��6/   .[�5  &   �� .��5  *   �� .��5  .   W� $# LogoutImageScale��6>   $$ ScaleX�6g   % 16�   $& 0:0:0.3D�6�   W� 5  .   $# LogoutImageScale��6>   $$ ScaleY�6g   % 16�   $& 0:0:0.3D�6�   5  +   5  '   5  #   5     5     5     5     5      � 5#  
   $��1��6   $��15,0,0,0q�6   ' � RowDefinitions' 5$     �� 5%     $��73 �6    �� 5&     $��* �6    .+�5*     �� $��8,8,0,0q�6   ���#DEECF8$ 10��6;   $	��420��6M   $��50��6Y   $
��Left=�6e   $	��Top=�6�   ._�5+     �� 
������ $ 
Horizontal=�66   $
��Left=�6O   $��Center=�6j   .+�5,     �� $!��/Resources/Leading-icon.png��6   $��45��6A   �� 5-     $��/Fonts/#Pontano Sans)�6   $��20(�6@   $��Center=�6N   ���#DEECF8$	��328��6~   $��42��6�   
������ $��Center=�6�   �� 5.     ���#DEECF8$
��Left=�62   $	��Top=�6M   $��45��6e   $��45��6p   $��0q�6|   
������ .��5/     �� $��/Resources/filter.png��6    $��30��6?   50     51     52     � 55     -
   5        ( �Name$( TaskListView��55     $��1��6)   ���#ECF7FF$y�	Collapsed=�6K   ' 56     �� 57     $��70 �6$   �� 58     $��* �6$   .+�5;     �� -   5        $ FilterButtonRow��5;     $��0��62   $	��160��6?   $��40��6K   $ 10��6W   
�������$��Right=�6|   $ 	18,0,18,0q�6�   $��0,5,50,0q�6�   ._�5<     �� $ 
Horizontal=�6!   $��Center=�6:   .+�5=     �� $��FILTER��6$   $��/Fonts/#Pontano Sans)�62   $��23(�6T   $��Center=�6b   $��0,0,10,0q�6}   �� 5>     -   5        $ DropdownButton��5>  !   #��[  69   $��Stretch=�6d   $
 Hand��6�   $|�False��6�   .��5?     �� $��/Resources/arrdown.png��6$   $��35��6D   ) J� IsHitTestVisible$) False��6O   $��0,1,0,0q�6h   z�5@  "   �� 5A  &   -   5        HoverImageScale��$ 1Q�5A  N   $ 1Q�6Y   $  0.5,0.5&�5C  c   5D     ! 5E     :� 5F  "   $" 
MouseEnter��6/   .[�5G  &   �� .��5H  *   �� .��5I  .   W� $# HoverImageScale��6>   $$ ScaleX�6f   % 1.156�   $& 0:0:0.3D�6�   W� 5J  .   $# HoverImageScale��6>   $$ ScaleY�6f   % 1.156�   $& 0:0:0.3D�6�   5K  +   5L  '   5M  #   :� 5N  "   $" 
MouseLeave��6/   .[�5O  &   �� .��5P  *   �� .��5Q  .   W� $# HoverImageScale��6>   $$ ScaleX�6f   % 16�   $& 0:0:0.3D�6�   W� 5R  .   $# HoverImageScale��6>   $$ ScaleY�6f   % 16�   $& 0:0:0.3D�6�   5S  +   5T  '   5U  #   5W     � 5X     -   5        $��DropdownPopup��5X      * � 	StaysOpen$* False��6l   + � PlacementTarget+ 65   �� , �� ElementName$, DropdownButton��.��5Y     �� 
�������$��1q�69   
�������$ 10q�6`   ._�5Z  "   �� .@�5[  &   �� $��CATEGORY��62   $��18(�6E   
�������$��1q�6g   ��5\  *   �� 5]  .   " ��64   .�5^  2   ��  � ��
 ���� 6O   �� 5_  2    � ��
 �   �6O    5`  2   P� 5a  6   
 �@J�IsMouseOver$
 True��6U   . �5b  :   ��  � �� �#E0E0E06W   5c  7   5e  /   5g  '   �� 5h  &   $��PRIORITY��62   $��18(�6E   
�������$��1q�6g   �� 5i  &   $��DEADLINE��62   $��18(�6E   
�������$��1q�6g   5j  #   5k     5l     5m     5n     �� 5p     -   5        $ ListCreateButtonRow��5p     $��1��66   ���#F3FAFF$��
20,5,20,15q�6X   $ 5��6l   $ 10q�6}   ._�5q     � ' 5r     �� 5s     $��* �6,   �� 5t     $
��Auto �6,   .+�5v     &   System.Windows.Controls.DataGrid  -   5        -  Name$- TaskListTable��5v  #   $��0��6:   .   AutoGenerateColumns$. False��6G   /   CanUserAddRows$/ False��6c   
�������
������ $��0q�5w  D   0   	RowHeight$0 40Q�5x     1   HeadersVisibility$1 Column=�6&   2   HorizontalGridLinesBrush2 �#EAE7E73   VerticalGridLinesBrush3 �#EAE7E7$��10q�5y  8    5z     =  7System.Windows.Controls.Primitives.DataGridColumnHeader)  #System.Windows.Controls.DataGridRow*  $System.Windows.Controls.DataGridCell��%h  '        '  n    '  �    �� 5|  "   "  6(   .�5}  &   ��  � ��$ 18(�6A   �� 5~  &    � ��
 �����6C   �� 5  &    � ��
 �   �6C   �� 5�  &    � ��$ /Fonts/#Pontano Sans)�6C   �� 5�  &    � ��$ Center=�6S   �� 5�  &    � ��$ 0,5,0,5q�6@   �� 5�  &    � ��$ 1,0,1,3q�6H   �� 5�  &    � �� �#EAE7E76D   5�  #   �� 5�  "   "  6(   .�5�  &   ��  � ��
 �����6C   5�  #   �� 5�  "   "  6(   .�5�  &   ��  � ��$ 0q�6H   �� 5�  &    � ��$ 5q�6@   �� 5�  &    � ��$ Center=�6S   5�  #   5�     4   Columns4 5�     4  .System.Windows.Controls.DataGridTemplateColumn  5�  "   ,  &System.Windows.Controls.DataGridColumn5   Width5	  /System.Windows.Controls.DataGridLengthConverter$5 50	 69   6   CellTemplate6 5�  &   �� 5�  *   .R�5�  .   �� $��20��65   $��20��6@   $	��✎��6L   $��12(�6X   
������ 
������ 
���   �$
 Hand��5�  >   5�  +   5�  #   0
  *System.Windows.Controls.DataGridTextColumn
  5�  "   7   Header$7 	Task Name��65   $	5 250	 6e   1  +System.Windows.Controls.DataGridBoundColumn8   Binding8 6H   �� *
TaskName+6e   
  5�  "   $7 Description��65   $	5 270	 6g   8 6J   �� *
Category+6g   
  5�  "   $7 Priority��65   $5 *	 6d   8 6G   �� *
Priority+9   ElementStyle9 5�  &   �� 5�  *   " ��60   .�5�  .   ��  � ��
 �  ��6K   �� 5�  .    � ��$
 Bold"�6K   �� 5�  .    � ��$ Center=�6T   5�  +   5�  #   
  5�  "   $7 Deadline��65   $5 *	 6d   8 6G   �� *
Deadline+6d   
  5�  "   $7 Status��65   $5 *	 6`   8 6E   �� *Status+6`     5�  "   $5 50	 69   6 5�  &   �� 5�  *   .R�5�  .   �� $��Center=�67   $��Center=�6T   5�  +   5�  #   5�     �� 5�     -   5        $ CreateTaskButton��5�  !   $��1��6;   #��[ 6H   $��CREATE TASK��6s   $	��140��5�     $��40��6!   $
 Hand��6-   6;   5�     5�     5�     � 5�     -   5        $( CreateTaskView��5�     $��1��6+   ���#ECF7FF$y�Visible=�6M   ' 5�     �� 5�     $��55 �6$   �� 5�     $��* �6$   .+�5�     � $��0��6   ���#ECF7FF' 5�     �� 5�     $��60 �6(   �� 5�     $��* �6(   .+�5�     � $��0��6    5�     �� 5�     $��* �6/   �� 5�     $��50 �6/   .+�5�     �� $��
13,10,13,0q�6%   $��0��69   .+�5�     �� $��CREATE TASK��6(   $��25(�6;   $!��/Fonts/#Pontano Sans Medium)�6I   $
��Left=�6r   $	��Top=�6�   $��0,10,0,0q�6�   5�     �� 5�     $��1��6%   $��Center=�65   $��Center=�6R   $��0,0,10,0q�6m   .+�5�     �� -   5        $ CancelButton��5�  %   #��[  6;   $
 Hand��6f   .��5�  "   �� $��/Resources/X (1).png��6(   5�     5�     5�     5�     �� 5�     -   5        $ addtaskborder��5�     $��1��60   ���#F3FAFF$��
20,5,20,15q�6R   $ 17��6f   $ 10q�6x   ._�5�     � ' 5�     �� 5�     $	��170 �6,   �� 5�     $��* �6,   .+�5�     �� $��0��6!   
�������$ 8��6A   $��	10,5,10,5q�6R   ._�5�     �  5�  "   �� 5�  &   $��* �67   �� 5�  &   $��* �67   .+�5�  "   �� $��0��6-   $ Vertical=�6=   $
��Left=�6T   $��	10,8,0,10q�6o   .+�5�  &   �� $��	Task Name��60   $!��/Fonts/#Pontano Sans Medium)�6A   $��20(�6j   $	��100��6y   $
��Left=�6�   $��Stretch=�6�   ���#7170706�   �� 5�  &   -   5        : ��Name$: TaskNameTxt��5�  .   $��
Assignment��6C   $!��/Fonts/#Pontano Sans Medium)�6U   $��22(�6~   $	��328��6�   $
��Left=�6�   $��30��6�   
������ 6�   �� 5�  &   $��Category��60   $!��/Fonts/#Pontano Sans Medium)�6@   $��20(�6j   $��90��6y   $��30��6�   $
��Left=�6�   ���#717070$��0,8,0,0q�6�   �� 5�  &   -   5        ; ��Name$; CategoryComboBox��5�  /   $!��/Fonts/#Pontano Sans Medium)�6I   $��38��6s   $	��220��6   
�������
�������$��0q�6�   $��4q�6�   $��Center=�6�   $
��Left=�6�   $��22(�6  .@�5�  *   �� $��Select Category��67   < �� 
IsSelected$
< True��6Q   �� 5�  *   $
��Work��67   �� 5�  *   $��Personal��67   5�  '   5�  #   �� 5�  "   $��1��6-   $ Vertical=�6=   $��Right=�6T   $��	0,8,10,10q�6p   .+�5�  &   �� $��Due Date��60   $!��/Fonts/#Pontano Sans Medium)�6@   $��20(�6i   $��85��6x   $��Right=�6�   $��Stretch=�6�   ���#7170706�   (  "System.Windows.Controls.DatePicker  5�  &   -   5        =  Name$= CustomDatePicker��5�  1   $!��/Fonts/#Pontano Sans Medium)�6K   $��35��6t   $	��190��6�   
�������
�������$��0q�5�     $��4q�61   $��Right=�6>   $��22(�6[   $��Center=�6j   $��Center=�6�    5�  *   :  4System.Windows.Controls.Primitives.DatePickerTextBox> �� 
IsReadOnly��%   '        �� 5�  .   "  64   .�5�  2   ��  � �� 5�  6   �� 5�  :   "  6J   .R�5�  >   �� $: PART_TextBox��6F   $��CHOOSE DATE��5�  2   
�������
���   �$
> True��5  2   
������ $��Center=�5  2   $��Center=�5  2   5  ;   5  3   5  /   5	  +   5
  '   �� 5  &   $��Category��60   $!��/Fonts/#Pontano Sans Medium)�6@   $��20(�6j   $��90��6y   $��30��6�   $��Right=�6�   ���#717070$��0,3,0,0q�6�   �� 5  &   -   5        $; PriorityComboBox��5  /   $!��/Fonts/#Pontano Sans Medium)�6I   $��38��6r   $	��220��6~   
�������
�������$��0q�6�   $��4q�6�   $��Center=�6�   $��Right=�6�   $��22(�6  ? �� SelectedIndex$? 0��6  .@�5  *   �� $��TYPE OF PRIORITY��67   $
< True��6R   
���   �6d   �� 5  *   $
��High��67   
���  ��6F   �� 5  *   $��Moderate��67   
��� ���6J   �� 5  *   $	��Low��67   
��� � �6E   5  '   5  #   5     5     �� 5     $��1��6%   $ Vertical=�62   $��	10,0,10,0q�6I   .+�5     �� $��Description��6(   $!��/Fonts/#Pontano Sans Medium)�6;   $��20(�6e   $	��110��6t   $��30��6�   $
��Left=�6�   ���#717070$��10,8,0,0q�6�   �� 5     $
��1015��6%   $	��250��62   $ 10��6?   
�������$ 0q�6d   $��0,0,0,18q�6p   ���#D1E8FF$��1q�6�   ._�5  "   �� -   5        $: DescriptionTxt��5  *   $
��1015��6B   $	��250��6O   $��	15,5,15,5q�6\   $��20(�6p   @ �� TextWrapping$
@ Wrap=�6~   
������ $��0q�6�   5     �� 5     -   5        $ AddTaskButton��5  %   $��1��6<   #��[ 6I   $��ADD TASK��6t   $	��100��6�   $��40��6�   $
 Hand��6�   5     5     5     5     5     5      5!     