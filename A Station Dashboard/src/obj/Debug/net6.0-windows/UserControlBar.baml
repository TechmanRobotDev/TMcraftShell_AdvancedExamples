   M S B A M L   `   `   `  ����D  @HDProcess, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:HDProcess	HDProcess  5          HDProcess.UserControlBar   5      W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      )localclr-namespace:HDProcess     5      -   5          >�Name$	  bar��5       � 	Resources 5
         nPlayBtn �� 	UriSource  hPlayBtn  	nPauseBtn  	hPauseBtn  nStopBtn  hStopBtn��%  &	        &	 N     &	 �     &	 �     &	 8    &	 �    �� 5   
   $5 //HDProcess;component/Images/btn-Play-normal.png?�6'   6e   �� 5   
   $4 ./HDProcess;component/Images/btn-Play-hover.png?�6'   6d   �� 5   
   $6 0/HDProcess;component/Images/btn-pause-normal.png?�6(   6g   �� 5   
   $5 //HDProcess;component/Images/btn-pause-Hover.png?�6(   6f   �� 5   
   $5 //HDProcess;component/Images/btn-Stop-normal.png?�6'   6e   �� 5   
   $4 ./HDProcess;component/Images/btn-Stop-Hover.png?�6'   6d   5      .��5      �� ._�5   
   �  � ColumnDefinitions 5      �� 5      6%   �� 5      6%   �� 5      6%   .+�5      ]� $��0��6   .+�5      �� -   5         ��Name$ PlayBtn��5      $��68��6@   $	��131��6L   ��5      �� 5       �� 
TargetType" ��6*   .R�5      �� #��z��6%   ._�5   "   �� 5       �� Triggers 5       P� 5!   "    P� Property � {� P� Value$
 True��6?   . �5"   &   �� 	 �� Property	 � ��
 �� Value
 5#   *   �� 5$   .    �� ImageSource# [  69    j� Stretch$ Uniform=�6a   6u   5&   '   5'   #   P� 5(   "    � {�$ False��6?   . �5)   &   �� 	 � ��
 5*   *   �� 5+   .   # [ 69   $ Uniform=�6a   6u   5-   '   5.   #   P� 5/   "    �@J�IsMouseOver$
 True��6A   . �50   &   �� 	 �@�Cursor$

 Hand��6?   6N   51   #   53      55      56      ]� 57      $��1��6   .+�58      �� -   5        $ PauseBtn��58      $��68��6B   $	��131��6N   ��59      �� 5:      " ��6*   .R�5;      �� #��z��6%   ._�5<   "   �� 5=       5>      P� 5?   "    � {�$
 True��6?   . �5@   &   �� 	 � ��
 5A   *   �� 5B   .   # [ 69   $ Uniform=�6b   6v   5D   '   5E   #   P� 5F   "    � {�$ False��6?   . �5G   &   �� 	 � ��
 5H   *   �� 5I   .   # [ 69   $ Uniform=�6b   6v   5K   '   5L   #   P� 5M   "    �@J�IsMouseOver$
 True��6A   . �5N   &   �� 	 �@�Cursor$

 Hand��6?   6N   5O   #   5Q      5S      5T      ]� 5U      $��2��6   .+�5V      �� -   5        $ StopBtn��5V      $��68��6@   $	��131��6L   ��5W      �� 5X      " ��6*   .R�5Y      �� #��z��6%   ._�5Z   "   �� 5[       5\      P� 5]   "    � {�$
 True��6?   . �5^   &   �� 	 � ��
 5_   *   �� 5`   .   # [ 69   $ Uniform=�6a   6u   5b   '   5c   #   P� 5d   "    � {�$ False��6?   . �5e   &   �� 	 � ��
 5f   *   �� 5g   .   # [ 69   $ Uniform=�6a   6u   5i   '   5j   #   P� 5k   "    �@J�IsMouseOver$
 True��6A   . �5l   &   �� 	 �@�Cursor$

 Hand��6?   6N   5m   #   5o      5q      5r      5s      5t      5u      