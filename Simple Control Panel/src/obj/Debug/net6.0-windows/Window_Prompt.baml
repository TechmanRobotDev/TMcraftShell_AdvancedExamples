   M S B A M L   `   `   `  ����E  ARunProject, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:RunProject
RunProject  5   	       RunProject.Window_Prompt   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:RunProject     5   	     � Title$  Window_Prompt��5   	   $	��450��6   $	��800��6,    � WindowStyle$
 None=�5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � AllowsTransparency$
 True��5   	   
������  J� Opacity$ 1Q�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � .+�5   
   �� $��Center=�6   $	��450��6/   $��Center=�6<   $	��800��6W    �� CornerRadius$ 30��6c   ���	#FFA4A4A4._�5      �� -   5         ��Name$ label��5      $����[Version 1.0.0.1]
Hint about this demo

- Make sure that there is a global variable(string array) in robot:
   g_MyProjectList{10} = {"ProjectName1", "ProjectName2",...,"ProjectName10"}

- The project list will be shown in drop-down list 
 
- Corresponded TMflow project should exist, ProjectName1, ProjetName2,...��6#   $
��Left=�6�  $	��341��6�  $��	49,66,0,0q�6�  $	��Top=�6	  $	��681��6!  $��Calibri)�6-  $��20(�6B  $
��Bold"�6P  
���������6u  Z� 5      �� 5   
   -   5        $��723,20,26,379q�5      ���	#FFFB5555$��X��6U   $��30(�6a   $
��Bold"�6o   ���	#FFFBFBFB$��0,0,0,0q�6�   ��6�   Z�  � 	Resources 5      	 �� 
TargetType
 �� Property �� Value��%d   '��       �� 5      "	 ��6   .�5      �� 
 �@��CornerRadius$ 40��65   6B   5      5      5      �� 5   
   -   5        $��698,347,10,10q�5      ���	#FFA4A4A4$�� ��6W   $��10(�6b   $
��Bold"�6p   ���	#FFFBFBFB$��0,0,0,0q�6�   ��6�   Z�  5      ��%_   '��       �� 5      "	 ��6   .�5      �� 
 �@��CornerRadius$ 40��65   5      5       5!      �� 5"   
   -   5         ��Name$ Button_ExitApp��5"      $��214,354,125,18q�6)   ���	#FFFB5555$4��.[Warning] Are you sure to terminate all Shell?��6i   $��20(�6�   $
��Bold"�6�   ���	#FFFBFBFB$��0,0,0,0q�6�   ��6�   Z�  5#      ��%_   '��       �� 5$      "	 ��6   .�5%      �� 
 �@��CornerRadius$ 40��65   5&      5'      5(      5*      5+      