   M S B A M L   `   `   `  ����  Minesweeper    Minesweeper.Gamescreen   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   >d2http://schemas.microsoft.com/expression/blend/2008    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  
Gamescreen��5      $	��456��6   $	��826��6'    � 
ResizeMode$ NoResize=�64    J� IsHitTestVisible . ���	#FF126748 � WindowStartupLocation$ CenterScreen=�6y    � 	Resources 5         ClickCommand �� Text��%(   &	        �� 5	   
   $
 Text��6/   5
       J� CommandBindings 5      )  #System.Windows.Input.CommandBinding  5   
   -   5           Command# [  5      6A    J� InputBindings 5      �� 5   
   
	 �� Key$	 C��6   
 �� 	Modifiers$

 Ctrlf�6    �� Command# [  6.   .��5      � .+�5   
   �� -   5        $��NumBdr��5      ���	#FF004B03$��3q�67   $��62��6K   $��5,27,5,0q�6W   $	��Top=�6i   ���	#FF27AC7B5      ]� 5   
   $��0��6   ��.  $��	0,0,0,393q�68   .+�5      o� -   5         o�Name$	 mnu��5      $	��Top=�6!   ���	#FF1B7C59$��25��6M   .@�5      m� -   5        $��
游戏(_G)��5      $��25��6E   .@�5      m� -   5        $��新游戏(_N)��5      60   m� 5      -   5        $��
退出(_X)��5      6/   5      m� 5      $��
关于(_A)��6   .@�5      m� -   5        $��关于游戏(_A)��5      61   5      5      5      � 5   
   $��81,38,161,339q�6   .+�5       �� -   5         ��Name$ Label_Level��5    �   $��关卡��6   $��32��6!   $
��Left=�6-   $��Right=�6H   $��Center=�6k   $	��Top=�6�   $��72��6�   $
��Bold"�6�   $��14(�6�   $��0,1,0,0q�6�   �� 5!      -	   5        $ Label_Score��5!   �   $��分数��6   $
��Left=�6!   $��Right=�6<   $��Center=�6_   $��65��6�   $
��Bold"�6�   $��14(�6�    J� RenderTransformOrigin$ 0.479,0.011&�6�   $��32��6�   $	��Top=�6�   $��	116,1,0,0q�6	  �� 5"      -
   5        $ Label_MineNum��5"   �   $��当前雷数��6   $��32��6#   $
��Left=�6/   $��Right=�6J   $��Center=�6m   $	��Top=�6�   $��97��6�   $
��Bold"�6�   $��14(�6�   $��	269,1,0,0q�6�   �� 5#      -   5        $ 
Label_Life��5#   �   $��	生命值��6   $��32��6"   $
��Left=�6.   $��Right=�6I   $��Center=�6l   $	��Top=�6�   $��80��6�   $
��Bold"�6�   $��14(�6�   $��	432,1,0,0q�6�   �� 5$      -   5         ��Name$ btnlevel_ten��5$   ;   $��32��6   $
��Left=�6    $	��Top=�6Q   $��img/num0.png��6j   $��72,0,0,0q�6�   �� 5%      -   5        $ btnlevel_one��5%   ;   $��32��6   $
��Left=�6    $	��Top=�6Q   $��img/num0.png��6j   $��94,0,0,0q�6�   �� 5&      -   5        $ 
btnsrc_ths��5&   ;   $��32��6   $
��Left=�6    $	��Top=�6O   $��img/num0.png��6h   $��	181,0,0,0q�6~   �� 5'      -   5        $ 
btnsrc_hus��5'   ;   $��32��6   $
��Left=�6    $	��Top=�6O   $��img/num0.png��6h   $��	203,0,0,0q�6~   �� 5(      -   5        $ 
btnsrc_ten��5(   ;   $��32��6   $
��Left=�6    $	��Top=�6O   $��img/num0.png��6h   $��	225,0,0,0q�6~   �� 5)      -   5        $ 
btnsrc_one��5)   ;   $��32��6   $
��Left=�6    $	��Top=�6O   $��img/num0.png��6h   $��	247,0,0,0q�6~   �� 5*      -   5        $ btnboom_hus��5*   ;   $��32��6   $
��Left=�6    $	��Top=�6P   $��img/num0.png��6i   $��	366,1,0,0q�6   �� 5+      -   5        $ btnboom_ten��5+   ;   $��32��6   $
��Left=�6    $	��Top=�6P   $��img/num0.png��6i   $��	388,1,0,0q�6   �� 5,      -   5        $ btnboom_one��5,   ;   $��32��6   $
��Left=�6    $	��Top=�6P   $��img/num0.png��6i   $��	410,1,0,0q�6   �� 5-      -   5        $ btnlife_ten��5-   N   $��32��6   $
��Left=�6    $��	512,1,0,0q�6;   $	��Top=�6c   $��img/num0.png��6|   �� 5.      -   5        $ btnlife_one��5.   N   $��32��6   $
��Left=�6    $��	534,1,0,0q�6;   $	��Top=�6c   $��img/num0.png��6|   5/      � 50   
   -   5         �Name$ 	GridMines��50      $��5,92,5,5q�6#   51      52      