   M S B A M L   `   `   `  ???? C  ?JLClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulle7clr-namespace:JLClient.MVVM.ViewModel.Components.Editor)JLClient.MVVM.ViewModel.Components.Editor  O,clr-namespace:JLClient.Core.VisualComponentsJLClient.Core.VisualComponents  W0clr-namespace:JLClient.MVVM.View.Components.Base"JLClient.MVVM.View.Components.Base  0    *JLClient.MVVM.View.Components.Editor.Index   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NAvalonDock, Version=4.70.1.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     M	viewmodel7clr-namespace:JLClient.MVVM.ViewModel.Components.Editor     ?imIcon,clr-namespace:JLClient.Core.VisualComponents     Abase0clr-namespace:JLClient.MVVM.View.Components.Base       ? 	Resources     BoolToVisConverter<  6AvalonDock.Converters.InverseBoolToVisibilityConverter  InvBoolToVisConverter??%   &	        &	      ??   .??? $??Stretch=?$??Stretch=?.+?? $??Stretch=?$??Stretch=?????  ?? 
TargetType" ? ?? Triggers ??  ?? Binding ?? *IsManualDisplaying+ ?? Value True.a???  ?? Property ? ?? ?? Value$ 0??.+?? $??0??y??? *	IsLoading+ ?? 	Converter# [  ? ColumnDefinitions ?? $??2* ??? $??* ?	 ? RowDefinitions	 ?? $??* ?.+??? $??0?????? *ManualTreeVM+?? $??1??.+??? $'??!Стадия разработки??  
card-label#??[ ?? ???? *StepProgressVM+?? $3??-Автогенерация материала??#??[ ?? $??Center=?
 ?? Orientation$
 
Horizontal=?.+???   	combo-box#??[  ?? SelectedItem ?? *	SplitType+???? *
SplitTypes+??   jl-primary-button#??[ $1??+Сгенерировать материал?? ? Cursor$
 Hand?????? *AutoGenerateManualCommand+?? $/??)Управление материалом??#??[ ? $??Center=?.+??? #??[ $1??+Импортировать материал?????? *ImportManualCommand+?? #??[ $/??)Сформировать материал?????? *ExportManualCommand+?? #??[ $1??+Предпросмотр материала?????? *ShowManualCommand+?? $.??(Интеграция с сервером??#??[ y??? *
FullAccess+# [  ?? $
 
Horizontal=?y??? *
FullAccess+# [  .+??? #??[ $*??$Загрузить на сервер??$
 Hand?????? *UploadManualCommand+?? #??[ $*??$Обновить на сервере?????? *UpdateManualCommand+y??? *CanUpdateManual+# [  ?? $??Мои материалы??#??[ y??? *
FullAccess+# [  ?? 
?????? $??0q?  jl-list-item#??[ ???? *	MyManuals+ ?? *CurrentManual+???? .R??? $
 
Horizontal=?.+??? #??[ $
??Left=????? *title+4   .JLClient.MVVM.View.Components.Base.LoadSpinner  $??0??y??? *	IsLoading+# [  ?? y??? *IsManualDisplaying+# [  ???? *PdfViewerVM+